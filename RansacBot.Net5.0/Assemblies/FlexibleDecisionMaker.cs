using RansacBot.Ground;
using RansacBot.Trading;
using RansacsRealTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	class FlexibleDecisionMaker : ITickProcessor, IDecisionProvider
	{
		public IClosingProvider ClosingProvider { get; private set; }
		public ITradeWithStopProvider TradeWithStopProvider { get; private set; }


		public ITradeByVertexDecider Decider { get; set; }
		void SubscribeDicider()
		{
			decider.NewTrade -= filter.OnNewTrade;
		}
		public ITradeByVertexDecider decider;

		public ITradeFilter filter;
		public IStopPlacer stopPlacer;

		private Dictionary<SigmaType, RansacsCascade> usedRansacs = new();

		public FlexibleDecisionMaker()
		{
			
		}



		public void OnNewTick(Tick tick)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}
	}

	class LinearTradeFilter : MultipleFilter<ITradeFilter, Trade>, IList<ITradeFilter>, ITradeFilter
	{

		public event Action<Trade> NewTrade;
		public void OnNewTrade(Trade trade)
		{
			OnNewItem(trade);
		}

		protected override void ThrowNewItem(Trade item)
		{
			NewTrade?.Invoke(item);
		}
	}
}
