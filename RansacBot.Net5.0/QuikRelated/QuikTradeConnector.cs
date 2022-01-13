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
	class QuikTradeConnector
	{
		readonly static Quik quik = QuikContainer.quik;
		readonly Param param;
		readonly string accountId;
		Order? lastOrder = null;
		StopOrder? lastStop = null;

		public QuikTradeConnector(Param param)
		{
			this.param = param;
		}

		public void OnOrder(Order order)
		{
			if(lastOrder != null && order.TransID == lastOrder.TransID)
			{
				if(order.State == State.Completed)
				{
					lastOrder = null;
					lastStop = null;
				}
			}
		}

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			if(lastOrder == null)
			{
				if(quik.Orders.KillOrder(lastOrder).Result < 0) throw new Exception("couldn't kill last order");
				if(quik.StopOrders.KillStopOrder(lastStop).Result < 0) throw new Exception("couldn't kill last stop order");
			}
			lastOrder = quik.Orders.SendMarketOrder(param.classCode, param.secCode, accountId, tradeWithStop.GetOperation(), 1).Result;
			lastStop = new StopOrder()
			{
				Account = accountId,
				ClassCode = param.classCode,
				SecCode = param.secCode,
				StopOrderType = StopOrderType.StopLimit,
				Operation = tradeWithStop.stop.GetOperation(),
				Condition = tradeWithStop.GetStopCondition(),
				ConditionPrice = (decimal)tradeWithStop.price
			};
			if(quik.StopOrders.CreateStopOrder(lastStop).Result < 0) throw new Exception("couldn't send stop order");
		}

	}
	internal static class TradeExtention
	{
		public static Operation GetOperation(this RansacBot.Trading.Trade trade)
		{
			return trade.direction == TradeDirection.buy ? Operation.Buy : Operation.Sell;
		}
		public static Condition GetStopCondition(this TradeWithStop trade)
		{
			return trade.stop.direction == TradeDirection.buy ? Condition.MoreOrEqual : Condition.LessOrEqual;
		}
	}
}
