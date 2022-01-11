using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class TradeOnVertexWithStop : TradeWithStop
	{
		Tick vertex;
		public TradeOnVertexWithStop(Trade trade, double stopPrice, Tick vertex) : base(trade, stopPrice)
		{
			this.vertex = vertex;
		}
	}
}
