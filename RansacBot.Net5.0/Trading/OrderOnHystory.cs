using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	class TradeOrder
	{
		static int lastTransID = 0;
		public State state = State.Active;
		public readonly int transID;
		public double price;
		public TradeDirection direction;
		public TradeOrder(Trade trade)
		{
			transID = ++lastTransID;
			this.price = trade.price;
			this.direction = trade.direction;
		}
	}
}
