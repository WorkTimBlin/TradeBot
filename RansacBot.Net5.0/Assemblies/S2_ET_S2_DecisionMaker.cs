using RansacBot.Trading;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	class S2_ET_S2_DecisionMaker : ITickProcessor
	{
		public IClosingProvider ClosingProvider { get; private set; }
		public ITradeWithStopProvider TradeWithStopProvider { get; private set; }

		public void OnNewTick(Tick tick)
		{
			session.OnNewTick(tick);
		}

		RansacsSession session;
		public IVertexFinder vertexProvider { get => session.monkeyNFilter; }
		public RansacsCascade SCascade { get; private set; }
		public RansacsCascade ETCascade { get; private set; }
		ITradeByVertexDecider decider;
		ITradeFilter filter;
		IStopPlacer stopPlacer;
		IClosingProvider closer;

		public S2_ET_S2_DecisionMaker(bool useFilter = true, int n = 100)
		{
			session = new(n);
			SCascade = new(session.vertexes, SigmaType.Sigma, 1, 90);
			ETCascade = new(session.vertexes, SigmaType.Sigma, 3, 90);
			decider = new InvertedNDecider();
			session.monkeyNFilter.NewExtremum += decider.OnNewExtremum;
			stopPlacer = new MaximinStopPlacer(SCascade, 0);

			if (useFilter)
			{
				filter = new HigherLowerFilterOnRansac(ETCascade, 2);
				decider.NewTrade += filter.OnNewTrade;
				filter.NewTrade += stopPlacer.OnNewTrade;
			}
			else
			{
				decider.NewTrade += stopPlacer.OnNewTrade;
			}

			closer = new CloserOnRansacStops(SCascade, 0, 100);

			ClosingProvider = closer;
			TradeWithStopProvider = stopPlacer;
		}
	}
}
