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
	class QuikTradeConnector : ITradesHystory
	{
		public event TradeWithStopHandler NewTradeWithStop;
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;
		public readonly Param param;
		public readonly string accountId;
		readonly static Quik quik = QuikContainer.quik;
		Order? lastOrder = null;
		StopOrder? lastStop = null;
		TradeWithStop? lastTradeWithStop = null;
		SortedList<StopOrder> longStops = new(new StopOrderComparer((first, second) => first.ConditionPrice > second.ConditionPrice ? 1 : -1));
		SortedList<StopOrder> shortStops = new(new StopOrderComparer((first, second) => first.ConditionPrice < second.ConditionPrice ? 1 : -1));

		public QuikTradeConnector(Param param, string accountId)
		{
			this.param = param;
			this.accountId = accountId;
		}

		public void OnOrderChanged(Order order)
		{
			if(lastOrder != null && order.TransID == lastOrder.TransID)
			{
				if(order.State == State.Completed)
				{
					NewTradeWithStop?.Invoke(lastTradeWithStop ?? throw new Exception(
						"last trade with stop is null but last order is not"));

					lastOrder = null;
					lastStop = null;
				}
			}
		}
		public void OnStopOrderChanged(StopOrder stopOrder)
		{
			if(stopOrder.State == State.Completed)
			{
				if (stopOrder.IsLong())
				{
					ExecutedLongStop?.Invoke(stopOrder.ConditionPrice);
					longStops.Remove(stopOrder);
				}
				else
				{
					ExecutedShortStop?.Invoke(stopOrder.ConditionPrice);
					shortStops.Remove(stopOrder);
				} 
			}
		}
		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			KillLastUnexecutedOrder();
			SendMarketOrderWithStop(tradeWithStop);
		}

		public void ClosePercentOfLongs(double percent)
		{
			ClosePercentOfTrades(percent, longStops, this.KilledLongStop);
		}
		public void ClosePercentOfShorts(double percent)
		{
			ClosePercentOfTrades(percent, shortStops, this.KilledShortStop);
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
		void KillLastUnexecutedOrder()
		{
			if (lastOrder != null)
			{
				if (quik.Orders.KillOrder(lastOrder).Result < 0)
					throw new Exception("couldn't kill last order");
				if (quik.StopOrders.KillStopOrder(lastStop).Result < 0)
					throw new Exception("couldn't kill last stop order");
			}
		}
		void SendMarketOrderWithStop(TradeWithStop tradeWithStop)
		{
			SendMarketOrder(tradeWithStop);
			lastStop = BuildStopOrder(tradeWithStop);
			AddLastStopToActiveStops();
			SendStopOrder(lastStop);
		}
		void AddLastStopToActiveStops()
		{
			if ((lastStop ?? throw new Exception("tried to save last stop when it was null")).IsLong())
			{
				longStops.Add(lastStop);
			}
			else
			{
				shortStops.Add(lastStop);
			}
		}
		void SendMarketOrder(TradeWithStop tradeWithStop)
		{
			lastOrder = quik.Orders.SendMarketOrder(
				param.classCode, param.secCode, accountId, tradeWithStop.GetOperation(), 1).Result;
		}
		void SendStopOrder(StopOrder stopOrder)
		{
			if (quik.StopOrders.CreateStopOrder(lastStop).Result < 0)
				throw new Exception("couldn't send stop order");
		}
		StopOrder BuildStopOrder(TradeWithStop tradeWithStop)
		{
			return new StopOrder()
			{
				Account = accountId,
				ClassCode = param.classCode,
				SecCode = param.secCode,
				StopOrderType = StopOrderType.StopLimit,
				Operation = tradeWithStop.stop.GetOperation(),
				Condition = tradeWithStop.GetStopCondition(),
				ConditionPrice = (decimal)tradeWithStop.stop.price
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
