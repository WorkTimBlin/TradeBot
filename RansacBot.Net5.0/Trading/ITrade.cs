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

	public interface ITradeWithStopFilter : ITradeWithStopProvider, ITradeWithStopProcessor { }
	public interface ITradeWithStopProcessor
	{
		public void OnNewTradeWithStop(TradeWithStop trade);
	}
	public interface ITradeWithStopProvider
	{
		public event Action<TradeWithStop> NewTradeWithStop;
	}

	public interface IStopPlacer : ITradeWithStopProvider
	{
		void OnNewTrade(Trade trade);
	}
}
