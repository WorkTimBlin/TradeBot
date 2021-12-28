using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class InvertedNDecider:ITradeByVertexDecider
	{
		public event TradeHandler NewTrade;

		public void OnNewVertex(Tick tick, VertexType type) { }
		public void OnNewExtremum(Tick extrmum, VertexType vertexType, Tick current)
		{
			if(vertexType == VertexType.Low)
			{
				NewTrade?.Invoke(new Trade(current.PRICE, TradeDirection.sell));
			}
			if(vertexType == VertexType.High)
			{
				NewTrade?.Invoke(new Trade(current.PRICE, TradeDirection.buy));
			}
		}
	}
}
