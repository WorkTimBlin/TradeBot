using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	class HystoryOrder : Trade
	{
		public State state = State.Active;
		public long transID = 0;
		public double completionPrice;
		public HystoryOrder(Trade trade) : base(trade.price, trade.direction) { }
	}
}
