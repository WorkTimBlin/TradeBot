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
		public TradeWithStop(Trade trade, double stopPrice):base(trade.price, trade.direction)
		{
			this.stop = new Trade(stopPrice, Opposite(direction));
		}
		public TradeDirection Opposite(TradeDirection direction)
		{
			if (direction == TradeDirection.buy) return TradeDirection.sell;
			if (direction == TradeDirection.sell) return TradeDirection.buy;
			throw new Exception("direction is neither buy or sell!");
		}
	}
	public enum TradeDirection
	{
		buy,
		sell
	}
}
