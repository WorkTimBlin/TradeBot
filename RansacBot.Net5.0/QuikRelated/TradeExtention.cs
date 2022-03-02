using QuikSharp.DataStructures;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.QuikRelated
{
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
