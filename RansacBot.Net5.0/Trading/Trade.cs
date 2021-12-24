using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	public class Trade
	{
		public readonly double price;
		public readonly TradeDirection direction;
		public Trade(double price, TradeDirection direction)
		{
			this.direction = direction;
			this.price = price;
		}
	}
	public class TradeWithStop:Trade
	{
		public readonly Trade stop;
		public TradeWithStop(Trade trade, Trade stop):base(trade.price, trade.direction)
		{
			this.stop = stop;
		}
	}
	public enum TradeDirection
	{
		buy,
		sell
	}
}
