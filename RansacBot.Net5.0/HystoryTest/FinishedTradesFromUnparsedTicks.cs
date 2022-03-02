using RansacBot.Assemblies;
using RansacBot.Trading;
using RansacBot.Trading.Hystory.Infrastructure;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.HystoryTest
{
	class FinishedTradesFromUnparsedTicks
	{
		public event Action<FinishedTradesFromUnparsedTicks> ProgressChanged;

		public HystoryProcessorState State { get; private set; } = HystoryProcessorState.Created;
		public bool IsComplete { get => State == HystoryProcessorState.Finished; }
		public double ProgressPromille { get => numberOfProcessedTicks * 1000 / numberOfTicks; }
		private ulong numberOfTicks;
		private ulong numberOfProcessedTicks = 0;
		private bool useFilter;
		private IEnumerable<Tick> ticks;
		private IEnumerable<string> unparsedTicks;

		public FinishedTradesFromUnparsedTicks(bool useFilter, IEnumerable<string> unparsedTicks, ITicksParser parser)
		{
			this.useFilter = useFilter;
			this.unparsedTicks = unparsedTicks;
			ticks = new TicksLazySequentialParser(unparsedTicks, parser);
		}

		public void GetReady()
		{
			State = HystoryProcessorState.Counting;
			CountTicks();
			State = HystoryProcessorState.Ready;
			ProgressChanged?.Invoke(this);
		}

		private void CountTicks()
		{
			numberOfTicks = (ulong)unparsedTicks.Count();
		}

		public IEnumerable<FinishedTrade> GetAllFinishedTrades(int period = 0)
		{
			if (State < HystoryProcessorState.Ready) throw new Exception("not ready to start");
			if (State > HystoryProcessorState.Ready) throw new Exception("started already");
			State = HystoryProcessorState.Processing;
			if (period == 0) period = (int)(numberOfTicks / 1000);
			S2_ET_S2_DecisionMaker decisionMaker =
				new S2_ET_S2_DecisionMaker(useFilter);

			HystoryTradingModule tradingModule =
				new(
					decisionMaker.TradeWithStopProvider,
					decisionMaker.ClosingProvider);


			FinishedTradesProvider finishedTradesBuilder = new();
			tradingModule.TradeExecuted += finishedTradesBuilder.OnTradeOpend;
			tradingModule.TradeClosedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;
			tradingModule.StopExecutedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;


			FinishedTrade? finishedTrade = null;
			void SetFinishedTrade(FinishedTrade trade) => finishedTrade = trade;
			finishedTradesBuilder.NewTradeFinished += SetFinishedTrade;


			HystoryQuikSimulator quikSimulator = HystoryQuikSimulator.Instance;
			finishedTradesBuilder.NewTick += quikSimulator.OnNewTick;
			quikSimulator.NewTick += decisionMaker.OnNewTick;

			
			int count = 0;
			foreach (Tick tick in ticks)
			{
				finishedTradesBuilder.OnNewTick(tick);
				if (count > period)
				{
					count = 0;
					ProgressChanged?.Invoke(this);
				}
				numberOfProcessedTicks++;
				count++;
				if(finishedTrade != null)
				{
					yield return (FinishedTrade)finishedTrade;
					finishedTrade = null;
				}
			}
			finishedTradesBuilder.NewTradeFinished -= SetFinishedTrade;
			State = HystoryProcessorState.Finished;
			ProgressChanged?.Invoke(this);
		}
	}
	public enum HystoryProcessorState
	{
		Created,
		Counting,
		Ready,
		Processing,
		Finished
	}
}
