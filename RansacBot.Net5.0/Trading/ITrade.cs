using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	public delegate void TradeHandler(Trade trade);
	public interface ITradeByVertexDecider
	{
		void OnNewExtremum(Tick extremum, VertexType vertexType, Tick current);
		void OnNewVertex(Tick tick, VertexType vertexType);
		public event TradeHandler NewTrade;
	}
	public interface ITradeFilter
	{
		void OnNewTrade(Trade trade);
		public event TradeHandler NewTrade;
	}

	public interface ITradeHystory
	{

	}
}
