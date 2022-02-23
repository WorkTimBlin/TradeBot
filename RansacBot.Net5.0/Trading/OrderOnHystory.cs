using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	class HystoryOrder
	{
		static int lastTransID = 0;
		public State state = State.Active;
		public readonly int transID;
		public readonly Trade trade;
		public double completionPrice;
		public HystoryOrder(Trade trade)
		{
			transID = ++lastTransID;
			this.trade = trade;
		}
	}
}
