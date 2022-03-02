using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacBot.Ground;
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
	public interface ITradeFilter : IItemFilter<Trade>
	{
		void OnNewTrade(Trade trade);
		public event Action<Trade> NewTrade;
		event Action<Trade> IItemProvider<Trade>.NewItem { add => NewTrade += value; remove => NewTrade -= value; }
		Action<Trade> IItemProcessor<Trade>.Processor { get => OnNewTrade; }
		void IItemProvider<Trade>.Subscribe(Action<Trade> action)
		{
			NewTrade += action;
		}
		void IItemProvider<Trade>.Unsubscribe(Action<Trade> action)
		{
			NewTrade -= action;
		}
	}

	public interface ITradeWithStopFilter : ITradeWithStopProvider, ITradeWithStopProcessor { }
	public interface ITradeWithStopProcessor : IItemProcessor<TradeWithStop>
	{
		public void OnNewTradeWithStop(TradeWithStop trade);
		Action<TradeWithStop> IItemProcessor<TradeWithStop>.Processor { get => this.OnNewTradeWithStop; }
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
