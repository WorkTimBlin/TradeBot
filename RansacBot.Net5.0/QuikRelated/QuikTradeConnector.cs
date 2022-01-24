using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	class TradeParams
	{
		public readonly string classCode;
		public readonly string secCode;
		public readonly string accountId;
		public readonly string clientCode;
		public TradeParams(string classCode, string secCode, string accountId, string clientCode)
		{
			this.classCode = classCode;
			this.secCode = secCode;
			this.accountId = accountId;
			this.clientCode = clientCode;
		}
	}
	class QuikTradeConnector : ITradesHystory, ITradeWithStopFilter
	{
		public event TradeWithStopHandler NewTradeWithStop;
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;
		public readonly Param param;
		public readonly string accountId;
		public readonly string clientCode = "53023";
		public bool IsLastOrderExecuted { get; private set; } = false;
		readonly static Quik quik = QuikContainer.Quik;
		Order? lastOrder = null;
		Order? executedOrder = null;
		StopOrder? lastStop = null;
		TradeWithStop? lastTradeWithStop = null;
		SortedList<StopOrder> longStops = new(new StopOrderComparer((first, second) => first.ConditionPrice > second.ConditionPrice ? 1 : -1));
		SortedList<StopOrder> shortStops = new(new StopOrderComparer((first, second) => first.ConditionPrice < second.ConditionPrice ? 1 : -1));
		Task? lastStopConfirmer = null;
		int timeoutMillseconds = 10000;

		public QuikTradeConnector(Param param, string accountId)
		{
			this.param = param;
			this.accountId = accountId;
			quik.Events.OnOrder += OnOrderChanged;
			//quik.Events.OnTrade += OnTrade;
			quik.Events.OnStopOrder += OnStopOrderChanged;
		}

		void OnOrderChanged(Order order)
		{
			Console.WriteLine("OnOrderChanged");
			if (order.State == State.Completed) executedOrder = order;
			CheckIfLastOrderExecuted();
		}
		void CheckIfLastOrderExecuted()
		{
			if (lastOrder != null && executedOrder != null && executedOrder.TransID == lastOrder.TransID)
			{
				NewTradeWithStop?.Invoke(lastTradeWithStop ?? throw new Exception(
					"last trade with stop is null but last order is not"));
				AddLastStopToActiveStops();

				lastOrder = null;
				lastStop = null;
				executedOrder = null;
				IsLastOrderExecuted = true;
			}
		}
		void OnTrade(QuikSharp.DataStructures.Transaction.Trade trade)
		{
			Order? linkedOrder = quik.Orders.GetOrder_by_Number(trade.OrderNum).Result;
			if(lastOrder != null)
			{
				OnOrderChanged(linkedOrder);
			}
			else
			{
				executedOrder = linkedOrder;
			}
			Console.WriteLine("OnTrade");
		}
		public void OnStopOrderChanged(StopOrder stopOrder)
		{
			Console.WriteLine("OnStopOrderChanged");
			if(stopOrder.State == State.Completed)
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
		void RemoveStopByTransId(StopOrder stopOrder, SortedList<StopOrder> stopOrders)
		{
			stopOrders.RemoveAt(
				stopOrders.FindIndex(
					(StopOrder match) => { return (match.TransId == stopOrder.TransId); }
					)
				);
		}
		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			if (!CanOpenTrade(tradeWithStop))
			{
				Console.WriteLine("can't open " + tradeWithStop.direction.ToString() + ", as reached limit");
				return;
			}
			lastTradeWithStop = tradeWithStop;
			KillLastUnexecutedOrderAndItsStop();
			SendLimitOrderWithStop(tradeWithStop);
			CheckIfLastOrderExecuted();
		}

		public void ClosePercentOfLongs(double percent)
		{
			ClosePercentOfTrades(percent, longStops, this.KilledLongStop);
		}
		public void ClosePercentOfShorts(double percent)
		{
			ClosePercentOfTrades(percent, shortStops, this.KilledShortStop);
		}

		bool CanOpenTrade(TradeWithStop tradeWithStop)
		{
			ParamNames checkingPriceParam;
			if (tradeWithStop.direction == TradeDirection.buy) checkingPriceParam = ParamNames.HIGH;
			else checkingPriceParam = ParamNames.LOW;
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
					tradeWithStop.direction == TradeDirection.buy,
					false).Result.Qty > 0);
		}
		void ClosePercentOfTrades(double percent, SortedList<StopOrder> stopOrders, ClosePosHandler closePosHandler)
		{
			for(int i = stopOrders.Count - 1; i > (int)(stopOrders.Count * (100 - percent) / 100) - 1; i--)
			{
				closePosHandler?.Invoke(stopOrders[i].ConditionPrice);
				quik.Orders.SendMarketOrder(param.classCode, param.secCode, accountId, stopOrders[i].Operation, 1);
			}
			KillLastPercent(percent, stopOrders);
		}
		void KillLastUnexecutedOrderAndItsStop()
		{
			if (lastOrder != null)
			{
				if (quik.Orders.KillOrder(lastOrder).Result < 0)
					throw new Exception("couldn't kill last order");
				if(lastStopConfirmer != null) if (!lastStopConfirmer.Wait(timeoutMillseconds)) throw new Exception("didn't confirm last stop");
				if (quik.StopOrders.KillStopOrder(lastStop).Result < 0)
					throw new Exception("couldn't kill last stop order");
				lastOrder = null;
				lastStop = null;
			}
		}
		void SendMarketOrderWithStop(TradeWithStop tradeWithStop)
		{
			SendMarketOrder(tradeWithStop);
			SendAndConfirmStopOrder(BuildStopOrder(tradeWithStop));
		}
		void SendLimitOrderWithStop(TradeWithStop tradeWithStop)
		{
			lastOrder = SendLimitOrder(tradeWithStop);
			IsLastOrderExecuted = false;
			SendAndConfirmStopOrder(BuildStopOrder(tradeWithStop));
		}
		void AddLastStopToActiveStops()
		{
			if(!lastStopConfirmer.Wait(1000)) throw new Exception();
			if ((lastStop ?? throw new Exception("tried to save last stop when it was null")).IsLong())
			{
				longStops.Add(lastStop);
			}
			else
			{
				shortStops.Add(lastStop);
			}
		}
		Order SendMarketOrder(TradeWithStop tradeWithStop)
		{
			return quik.Orders.SendMarketOrder(
				param.classCode, param.secCode, accountId, tradeWithStop.GetOperation(), 1).Result;
		}
		Order SendLimitOrder(TradeWithStop tradeWithStop)
		{
			return quik.Orders.SendLimitOrder(
				param.classCode, 
				param.secCode, 
				accountId, 
				tradeWithStop.GetOperation(), 
				(decimal)tradeWithStop.price, 
				1).Result;
		}
		void SendAndConfirmStopOrder(StopOrder stopOrder)
		{
			void OnStopOrder(StopOrder stopOrderFromQuik)
			{
				Console.WriteLine("StopOrderCallback");
				if (AreStopOrdersMatching(stopOrder, stopOrderFromQuik))
				{
					Console.WriteLine("stopOrder confirmed");
					lastStop = stopOrderFromQuik;
					quik.Events.OnStopOrder -= OnStopOrder;
				}
			}
			Console.WriteLine(DateTime.Now.Ticks.ToString() + " subscribed callback catcher");
			quik.Events.OnStopOrder += OnStopOrder;
			if (quik.StopOrders.CreateStopOrder(stopOrder).Result < 0)
			{
				quik.Events.OnStopOrder -= OnStopOrder;
				Console.WriteLine("unsubscribed callback catcher as couldn't send");
				throw new Exception("couldn't send stop order");
			}
			lastStopConfirmer = Task.Run(() =>
				{
					while (lastStop == null) ;
				});
		}
		bool AreStopOrdersMatching(StopOrder sent, StopOrder fromQuik)
		{
			return (
				sent.Account == fromQuik.Account &&
				sent.ClassCode == fromQuik.ClassCode &&
				sent.SecCode == fromQuik.SecCode &&
				sent.StopOrderType == fromQuik.StopOrderType &&
				sent.Operation == fromQuik.Operation &&
				sent.Condition == fromQuik.Condition &&
				sent.ConditionPrice == fromQuik.ConditionPrice &&
				sent.Price == fromQuik.Price &&
				sent.Quantity == fromQuik.Quantity &&
				sent.ClientCode == fromQuik.ClientCode
				);
		}
		public StopOrder BuildStopOrder(TradeWithStop tradeWithStop)
		{
			return new StopOrder()
			{
				Account = accountId,
				ClassCode = param.classCode,
				SecCode = param.secCode,
				StopOrderType = StopOrderType.StopLimit,
				Operation = tradeWithStop.stop.GetOperation(),
				Condition = tradeWithStop.GetStopCondition(),
				ConditionPrice = (decimal)tradeWithStop.stop.price,
				Price = (decimal)tradeWithStop.stop.price,
				Quantity = 1,
				ClientCode = "SPBFUT005gx"
			};
		}

		void KillLongStops(double percent)
		{
			KillLastPercent(percent, longStops);
		}
		void KillShortStops(double percent)
		{
			KillLastPercent(percent, shortStops);
		}
		void KillLastPercent(double percent, SortedList<StopOrder> orders)
		{
			for(int i = (int)(orders.Count * (100 - percent) / 100); i < orders.Count; i++)
			{
				long s = quik.StopOrders.KillStopOrder(orders[i]).Result;
			}
			orders.RemoveRange((int)(orders.Count * (100 - percent) / 100), orders.Count - (int)(orders.Count * (100 - percent) / 100));
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
	internal static class TradeExtention
	{
		public static Operation GetOperation(this Trading.Trade trade)
		{
			return trade.direction == TradeDirection.buy ? Operation.Buy : Operation.Sell;
		}
		public static Condition GetStopCondition(this TradeWithStop trade)
		{
			return trade.stop.direction == TradeDirection.buy ? 
				Condition.MoreOrEqual : Condition.LessOrEqual;
		}
		public static bool IsLong(this StopOrder stopOrder)
		{
			return stopOrder.Operation == Operation.Sell;
		}
	}
}
