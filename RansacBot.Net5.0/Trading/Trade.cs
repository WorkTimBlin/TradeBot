using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	public readonly struct Trade
	{
		readonly double price;
		readonly TradeDirection direction;
	}
	public enum TradeDirection
	{
		buy,
		sell
	}
}
