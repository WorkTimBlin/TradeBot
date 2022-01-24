using QuikSharp.DataStructures;
using QuikSharp;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;

namespace RansacBot.QuikRelated
{
	class TradeWithStopEnsurer : ITradeWithStopFilter
	{
		public event TradeWithStopHandler NewTradeWithStop;
		public event StopOrderHandler NewStopOrder;

		public Order Order { get; private set; }
		public StopOrder StopOrder { get; private set; }
		public TradeWithStop TradeWithStop { get; private set; }
		public OrganisedState StateOrder { get; private set; } = OrganisedState.Local;
		public OrganisedState StateStopOrder { get; private set; } = OrganisedState.Local;
		private readonly TradeParams tradeParams;
		private readonly Quik quik;
		private StopStorage stopStorage;

		public TradeWithStopEnsurer(Quik quik, TradeParams tradeParams, StopStorage stopStorage)
		{
			this.quik = quik;
			this.tradeParams = tradeParams;
			quik.Events.OnOrder += OnOrderChanged;
			quik.Events.OnStopOrder += OnStopOrderChanged;
			this.stopStorage = stopStorage;
		}

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			TradeWithStop = tradeWithStop;
			if (!CanDoNext()) KillCurrent();
			StateOrder = OrganisedState.Local;
			StateStopOrder = OrganisedState.Local;
			Order = BuildOrder(tradeWithStop);
			StopOrder = BuildStopOrder(tradeWithStop);
			if (CanOpenTradeWithStop(tradeWithStop))
			{
				SendOrder();
				SendStopOrder();
			}
		}

		void KillCurrent()
		{
			Console.WriteLine("Killing current..");
			while (StateOrder == OrganisedState.Sent) ;
			if (StateOrder == OrganisedState.Delivered)
			{
				Order order = quik.Orders.GetOrder_by_transID(
					tradeParams.classCode, 
					tradeParams.secCode, 
					Order.TransID).Result;
				if (order.State == State.Completed) return;
				if(quik.Orders.KillOrder(order).Result < 0)
					throw new Exception("couldn't kill last order");
				Console.WriteLine("Killed order " + Order.TransID);
			}
			while (StateStopOrder == OrganisedState.Sent) ;
			if (StateStopOrder == OrganisedState.Delivered)
			{
				if (quik.StopOrders.KillStopOrder(StopOrder).Result < 0)
					throw new Exception("couldn't kill last stop order");
				State stopState = StopOrder.State;
				StopOrder.State = State.Canceled;
				stopStorage.OnStopOrderChanged(StopOrder);
				StopOrder.State = stopState;
				Console.WriteLine("Killed stop order " + StopOrder.TransId);
			}
		}
		bool CanDoNext()
		{
			return !(StateOrder == OrganisedState.Delivered && StateStopOrder == OrganisedState.Delivered);
		}
		void SendOrder()
		{
			long transId = quik.Orders.CreateOrder(Order).Result;
			if (transId < 0) throw new Exception("couldn't send order");
			StateOrder = OrganisedState.Sent;
			Order.TransID = transId;
			Console.WriteLine("order " + Order.TransID.ToString() + " sent");
		}
		void SendStopOrder()
		{
			long transId = quik.StopOrders.CreateStopOrder(StopOrder).Result;
			if (transId < 0) throw new Exception("couldn't send order");
			StateStopOrder = OrganisedState.Sent;
			StopOrder.TransId = transId;
			Console.WriteLine("stop order " + StopOrder.TransId.ToString() + " sent");
		}
		void OnOrderChanged(Order order)
		{
			if (order.TransID != (Order ?? new Order()).TransID) return;
			StateOrder = OrganisedState.Delivered;
			this.Order = order;
			CheckForOrderCompletion();
		}
		void OnStopOrderChanged(StopOrder stopOrder)
		{
			if ((StopOrder??new StopOrder()).TransId != stopOrder.TransId) return;
			if (StateStopOrder != OrganisedState.Delivered)
			{
				stopStorage.OnNewStopOrder(stopOrder);
			}
			StateStopOrder = OrganisedState.Delivered;
			this.StopOrder = stopOrder;
			CheckForStopOrderCompletion();
		}

		void CheckForOrderCompletion()
		{
			Console.WriteLine("order " + Order.TransID.ToString() + " delivered" + ", State: " + Order.State.ToString());
			if (Order.State.Equals(State.Completed))
			{
				StateOrder = OrganisedState.Executed;
				InvokeTradeWithStopIfNeeded();
				Console.WriteLine("order " + Order.TransID.ToString() + " completed");
			}
			if (Order.State == QuikSharp.DataStructures.State.Canceled)
			{
				StateOrder = OrganisedState.Canceled;
				Console.WriteLine("order " + Order.TransID.ToString() + " canceled");
			}
		}
		void CheckForStopOrderCompletion()
		{
			Console.WriteLine("stop order " + StopOrder.TransId.ToString() + " delivered");
			InvokeTradeWithStopIfNeeded();
			if (StopOrder.State == State.Completed)
			{
				StateStopOrder = OrganisedState.Executed;
				stopStorage.OnStopOrderChanged(StopOrder);
				Console.WriteLine("stop order " + StopOrder.TransId.ToString() + " executed");
			}
			if (StopOrder.State == State.Canceled)
			{
				StateStopOrder = OrganisedState.Canceled;
				stopStorage.OnStopOrderChanged(StopOrder);
				Console.WriteLine("stop order " + StopOrder.TransId.ToString() + " canceled");
			}
		}
		void InvokeTradeWithStopIfNeeded()
		{
			if (StateOrder == OrganisedState.Executed && StateStopOrder == OrganisedState.Delivered)
			{
				NewTradeWithStop?.Invoke(TradeWithStop ?? throw new Exception("tradeWithStop is null but order isn't"));
			}
		}
		bool CanOpenTradeWithStop(TradeWithStop tradeWithStop)
		{
			return (CanOpenTrade(tradeWithStop) && CanOpenTrade(tradeWithStop.stop));
		}

		bool CanOpenTrade(Trading.Trade trade)
		{
			ParamNames checkingPriceParam;
			if (trade.direction == TradeDirection.buy) checkingPriceParam = ParamNames.HIGH;
			else checkingPriceParam = ParamNames.LOW;
			int i = quik.Trading.CalcBuySell(
					"SPBFUT",
					"RIH2",
					"53023",
					"SPBFUT005gx",
					Double.Parse(quik.Trading.GetParamEx(
						"SPBFUT",
						"RIH2",
						checkingPriceParam
						).Result.ParamValue, System.Globalization.CultureInfo.InvariantCulture),
					trade.direction == TradeDirection.buy,
					false).Result.Qty;
			return (quik.Trading.CalcBuySell(
					"SPBFUT",
					"RIH2",
					"53023",
					"SPBFUT005gx",
					Double.Parse(quik.Trading.GetParamEx(
						"SPBFUT",
						"RIH2",
						checkingPriceParam
						).Result.ParamValue, System.Globalization.CultureInfo.InvariantCulture),
					trade.direction == TradeDirection.buy,
					false).Result.Qty > 0);
		}

		Order BuildOrder(Trading.Trade trade)
		{
			return new Order()
			{
				ClassCode = tradeParams.classCode,
				SecCode = tradeParams.secCode,
				Account = tradeParams.accountId,
				Operation = trade.GetOperation(),
				Price = (decimal)trade.price,
				ClientCode = tradeParams.clientCode,
				Quantity = 1
			};
		}

		StopOrder BuildStopOrder(TradeWithStop tradeWithStop)
		{
			return new StopOrder()
			{
				Account = tradeParams.accountId,
				ClassCode = tradeParams.classCode,
				SecCode = tradeParams.secCode,
				StopOrderType = StopOrderType.StopLimit,
				Operation = tradeWithStop.stop.GetOperation(),
				Condition = tradeWithStop.GetStopCondition(),
				ConditionPrice = (decimal)tradeWithStop.stop.price,
				Price = (decimal)tradeWithStop.stop.price,
				Quantity = 1,
				ClientCode = tradeParams.clientCode
			};
		}

		public enum OrganisedState:byte
		{
			Local,
			Sent,
			Delivered,
			Executed,
			Canceled
		}
	}

	class StopStorage : ITradesHystory
	{
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;

		private readonly TradeParams tradeParams;
		private readonly Quik quik;

		SortedList<StopOrder> longStops = new(new StopOrderComparer((first, second) => first.ConditionPrice > second.ConditionPrice ? 1 : -1));
		SortedList<StopOrder> shortStops = new(new StopOrderComparer((first, second) => first.ConditionPrice < second.ConditionPrice ? 1 : -1));

		public StopStorage(Quik quik, TradeParams tradeParams)
		{
			this.quik = quik;
			this.tradeParams = tradeParams;
			quik.Events.OnStopOrder += OnStopOrderChanged;
		}
		public void OnNewStopOrder(StopOrder stopOrder)
		{
			if (stopOrder.IsLong()) longStops.Add(stopOrder);
			else shortStops.Add(stopOrder);
		}
		public void OnStopOrderChanged(StopOrder stopOrder)
		{
			SortedList<StopOrder> currentList = stopOrder.IsLong() ? longStops : shortStops;
			int indexOfOrder = currentList.FindIndex(
				(StopOrder stopOrderInList) => 
				{ return stopOrder.TransId == stopOrderInList.TransId; }
			);
			if(indexOfOrder > -1)
			{
				currentList[indexOfOrder] = stopOrder;
				if (stopOrder.State == State.Completed || stopOrder.State == State.Canceled)
				{
					ClosePosHandler handler = 
						stopOrder.State == State.Completed?
							stopOrder.IsLong() ? ExecutedLongStop : ExecutedShortStop :
							stopOrder.IsLong() ? KilledLongStop : KilledShortStop;
					handler?.Invoke(stopOrder.ConditionPrice);
					currentList.RemoveAt(indexOfOrder);
				}
			}
		}
		public void ClosePercentOfLongs(double percent)
		{
			ClosePercentOfTrades(percent, longStops, KilledLongStop);
		}
		public void ClosePercentOfShorts(double percent)
		{
			ClosePercentOfTrades(percent, shortStops, KilledShortStop);
		}
		public List<string> GetLongs()
		{
			return new(longStops.Select((StopOrder stopOrder) => { return stopOrder.TransId.ToString() + " " + stopOrder.ConditionPrice.ToString(); }));
		}
		public List<string> GetShorts()
		{
			return new(shortStops.Select((StopOrder stopOrder) => { return stopOrder.TransId.ToString() + " " + stopOrder.ConditionPrice.ToString(); }));
		}
		void ClosePercentOfTrades(double percent, SortedList<StopOrder> stopOrders, ClosePosHandler KillHandler)
		{
			for (int i = stopOrders.Count - 1; i > (int)(stopOrders.Count * (100 - percent) / 100) - 1; i--)
			{
				quik.Orders.SendMarketOrder(tradeParams.classCode, tradeParams.secCode, tradeParams.accountId, stopOrders[i].Operation, 1).Start();
			}
			KillLastPercent(percent, stopOrders);
		}
		void KillLastPercent(double percent, SortedList<StopOrder> orders)
		{
			for (int i = (int)(orders.Count * (100 - percent) / 100); i < orders.Count; i++)
			{
				quik.StopOrders.KillStopOrder(orders[i]).Start();
			}
		}
		void RemoveStopByTransId(StopOrder stopOrder, SortedList<StopOrder> stopOrders)
		{
			stopOrders.RemoveAt(
				stopOrders.FindIndex(
					(StopOrder match) => { return (match.TransId == stopOrder.TransId); }
					)
				);
		}
		private class StopOrderComparer : IComparer<StopOrder>
		{
			private Func<StopOrder, StopOrder, int> compare;
			public StopOrderComparer(Func<StopOrder, StopOrder, int> compare)
			{
				this.compare = compare;
			}

			public int Compare(StopOrder first, StopOrder second)
			{
				return compare(first, second);
			}
		}
	}
}
