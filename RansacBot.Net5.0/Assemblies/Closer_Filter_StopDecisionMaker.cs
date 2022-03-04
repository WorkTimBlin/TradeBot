using RansacBot.Trading;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	class Closer_Filter_StopDecisionMaker : ITickProcessor, IDecisionProvider
	{
		public IClosingProvider ClosingProvider => closer;
		public ITradeWithStopProvider TradeWithStopProvider => stopPlacer;
		public IVertexFinder VertexProvider => session.monkeyNFilter;


		RansacsSession session;
		ITradeByVertexDecider decider;
		ITradeFilter filter;
		IStopPlacer stopPlacer;
		IClosingProvider closer;
		readonly bool usingFilter;
		readonly double n;
		readonly RansacObservingParameters closingRansac;
		readonly RansacObservingParameters filterRansac;
		readonly RansacObservingParameters stopsRansac;
		readonly List<RansacsCascade?> cascades = new() { null, null, null, null };

		public Closer_Filter_StopDecisionMaker(
			RansacObservingParameters closingRansac, 
			RansacObservingParameters filterRansac, 
			RansacObservingParameters stopsRansac,
			bool useFilter = true, double n = 100)
		{
			this.closingRansac = closingRansac;
			this.filterRansac = filterRansac;
			this.stopsRansac = stopsRansac;
			usingFilter = useFilter;
			this.n = n;
			Init();
		}


		public void OnNewTick(Tick tick)
		{
			session.OnNewTick(tick);
		}
		public void Clear()
		{
			if (usingFilter) UnsubscribeThroughFilter();
			else UnsubscribeBypassingFilter();
			Init();
		}

		void Init()
		{
			session = new(n);
			SetCascades();
			SetNewDecider();
			SetNewStopPlacer();
			if (usingFilter)
			{
				SetNewFilter();
				SubscribeThroughFilter();
			}
			else
			{
				SubscribeBypassingFilter();
			}
			SetNewCloser();
		}
		void UnsubscribeBypassingFilter()
		{
			session.monkeyNFilter.NewExtremum -= decider.OnNewExtremum;
			decider.NewTrade -= stopPlacer.OnNewTrade;
		}
		void UnsubscribeThroughFilter()
		{
			session.monkeyNFilter.NewExtremum += decider.OnNewExtremum;
			decider.NewTrade += filter.OnNewTrade;
			filter.NewTrade += stopPlacer.OnNewTrade;
		}
		void SubscribeBypassingFilter()
		{
			session.monkeyNFilter.NewExtremum += decider.OnNewExtremum;
			decider.NewTrade += stopPlacer.OnNewTrade;
		}
		void SubscribeThroughFilter()
		{
			session.monkeyNFilter.NewExtremum += decider.OnNewExtremum;
			decider.NewTrade += filter.OnNewTrade;
			filter.NewTrade += stopPlacer.OnNewTrade;
		}
		void SetNewDecider()
		{
			decider = new InvertedNDecider();
		}
		void SetNewStopPlacer()
		{
			stopPlacer = new MaximinStopPlacer(GetCascade(stopsRansac), stopsRansac.level - 2);
		}
		void SetNewFilter()
		{
			filter = new HigherLowerFilterOnRansac(GetCascade(filterRansac), filterRansac.level - 2);
		}
		void SetNewCloser()
		{
			closer = new CloserOnRansacStops(GetCascade(closingRansac), closingRansac.level - 2, 100);
		}

		RansacsCascade GetCascade(RansacObservingParameters parameters)
		{
			return cascades[(int)parameters.sigmaType] ?? throw new Exception("No such cascade");
		}
		private void SetCascades()
		{
			SetCascades(new() { closingRansac, stopsRansac, filterRansac });
		}
		private void SetCascades(List<RansacObservingParameters> usedRansacLevels)
		{
			int[] levels = new int[4] { 0, 0, 0, 0 };
			foreach (RansacObservingParameters parameters in usedRansacLevels)
			{
				if (parameters.level > levels[(int)parameters.sigmaType])
				{
					levels[(int)parameters.sigmaType] = parameters.level;
				}
			}
			for(int i = 0; i < 4; i++)
			{
				cascades[i] = new(session.vertexes, (SigmaType)i, levels[i] - 1, 90);
			}
		}
	}
}
