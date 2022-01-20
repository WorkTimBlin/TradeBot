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
			if (!CanDoNext()) KillCurrent();
			Order = BuildOrder(tradeWithStop);
			StopOrder = BuildStopOrder(tradeWithStop);
			SendOrder();
			SendStopOrder();
			
		}

		void KillCurrent()
		{
			while (StateOrder == OrganisedState.Sent) ;
			if (StateOrder == OrganisedState.Delivered)
			{
				if(quik.Orders.KillOrder(Order).Result < 0)
					throw new Exception("couldn't kill last order");
			}
			while (StateStopOrder == OrganisedState.Sent) ;
			if (StateStopOrder == OrganisedState.Delivered)
			{
				if (quik.StopOrders.KillStopOrder(StopOrder).Result < 0)
					throw new Exception("couldn't kill last stop order");
			}
		}
		bool CanDoNext()
		{
			return StateOrder == OrganisedState.Executed && (StateStopOrder == OrganisedState.Delivered || StateStopOrder == OrganisedState.Executed);
		}
		void SendOrder()
		{
			long transId = quik.Orders.CreateOrder(Order).Result;
			StateOrder = OrganisedState.Sent;
			if (transId < 0) throw new Exception("couldn't send order");
			Order.TransID = transId;
		}
		void SendStopOrder()
		{
			long transId = quik.StopOrders.CreateStopOrder(StopOrder).Result;
			StateStopOrder = OrganisedState.Sent;
			if (transId < 0) throw new Exception("couldn't send order");
			StopOrder.TransId = transId;
		}
		void OnOrderChanged(Order order)
		{
			if (order.TransID != Order.TransID) return;
			StateOrder = OrganisedState.Delivered;
			this.Order = order;
			CheckForOrderCompletion();
		}
		void OnStopOrderChanged(StopOrder stopOrder)
		{
			if (StopOrder.TransId != stopOrder.TransId) return;
			StateStopOrder = OrganisedState.Delivered;
			stopStorage.OnNewStopOrder(stopOrder);
			this.StopOrder = stopOrder;
			CheckForStopOrderCompletion();
		}

		void CheckForOrderCompletion()
		{
			if (Order.State == State.Completed)
			{
				StateOrder = OrganisedState.Executed;
				InvokeTradeWithStopIfNeeded();
			}
			if (Order.State == State.Canceled)
			{
				StateOrder = OrganisedState.Canceled;
			}
		}
		void CheckForStopOrderCompletion()
		{
			InvokeTradeWithStopIfNeeded();
			if (StopOrder.State == State.Completed)
			{
				StateStopOrder = OrganisedState.Executed;
				stopStorage.OnStopOrderChanged(StopOrder);
			}
			if (StopOrder.State == State.Canceled)
			{
				StateStopOrder = OrganisedState.Canceled;
				stopStorage.OnStopOrderChanged(StopOrder);
			}

		}
		void InvokeTradeWithStopIfNeeded()
		{
			if (StateOrder == OrganisedState.Executed && StateStopOrder == OrganisedState.Delivered)
			{
				NewTradeWithStop?.Invoke(TradeWithStop ?? throw new Exception("tradeWithStop is null but order isn't"));
			}
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
			Console.WriteLine("OnStopOrderChanged");
			
			if (stopOrder.State == State.Completed)
			{
				if (stopOrder.IsLong())
				{
					ExecutedLongStop?.Invoke(stopOrder.ConditionPrice);
					RemoveStopByTransId(stopOrder, longStops);
				}
				else
				{
					ExecutedShortStop?.Invoke(stopOrder.ConditionPrice);
					RemoveStopByTransId(stopOrder, shortStops);
				}
			}
			Console.WriteLine("OnStopOrderChangedExiting");
			
		}
		public void ClosePercentOfLongs(double percent)
		{
			ClosePercentOfTrades(percent, longStops, KilledLongStop);
		}
		public void ClosePercentOfShorts(double percent)
		{
			ClosePercentOfTrades(percent, shortStops, KilledShortStop);
		}
		void ClosePercentOfTrades(double percent, SortedList<StopOrder> stopOrders, ClosePosHandler KillHandler)
		{
			for (int i = stopOrders.Count - 1; i > (int)(stopOrders.Count * (100 - percent) / 100) - 1; i--)
			{
				KillHandler?.Invoke(stopOrders[i].ConditionPrice);
				quik.Orders.SendMarketOrder(tradeParams.classCode, tradeParams.secCode, tradeParams.accountId, stopOrders[i].Operation, 1).Start();
			}
			KillLastPercent(percent, stopOrders);
		}
		//TODO: make sure outside killed stops are processed
		void KillLastPercent(double percent, SortedList<StopOrder> orders)
		{
			for (int i = (int)(orders.Count * (100 - percent) / 100); i < orders.Count; i++)
			{
				quik.StopOrders.KillStopOrder(orders[i]).Start();
			}
			orders.RemoveRange((int)(orders.Count * (100 - percent) / 100), orders.Count - (int)(orders.Count * (100 - percent) / 100));
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
