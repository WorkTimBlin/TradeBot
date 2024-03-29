﻿using RansacBot.Assemblies;
using RansacBot.Trading;
using RansacBot.Trading.Hystory.Infrastructure;
using RansacsRealTime;
using System;
using System.Collections;
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
		private IEnumerable<string> unparcedTicks;
		private Func<ITradingEnvironment, IDecisionProvider> decisionMakerGetter;

		public FinishedTradesFromUnparsedTicks(IEnumerable<string> unparsedTicks, Func<ITradingEnvironment, IDecisionProvider> decisionProvider)
		{
			decisionMakerGetter = decisionProvider;
			this.unparcedTicks = unparsedTicks;
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
			numberOfTicks = (ulong)unparcedTicks.Count();
		}

		public IEnumerable<FinishedTrade> GetAllFinishedTrades(int period = 0)
		{
			if (State < HystoryProcessorState.Ready) throw new Exception("not ready to start");
			if (State > HystoryProcessorState.Ready) throw new Exception("started already");
			State = HystoryProcessorState.Processing;
			if (period == 0) period = (int)(numberOfTicks / 1000);
			TicksDateTimeExtractor extractor = new(unparcedTicks.Select(TicksWithDateTimeParser.ParseTickDamir));
			IEnumerable<Tick> ticks = extractor;
			IDecisionProvider decisionMaker = decisionMakerGetter(extractor);

			HystoryTradingModule tradingModule =
				new(
					decisionMaker.TradeWithStopProvider,
					decisionMaker.ClosingProvider);


			FinishedTradesProvider finishedTradesBuilder = new();
			tradingModule.TradeExecuted += finishedTradesBuilder.OnTradeOpened;
			tradingModule.TradeClosedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;
			tradingModule.StopExecutedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;


			Queue<FinishedTrade> finishedTrades = new();
			void SetFinishedTrade(FinishedTrade trade) => finishedTrades.Enqueue(trade);
			finishedTradesBuilder.NewTradeFinished += SetFinishedTrade;


			HystoryQuikSimulator quikSimulator = tradingModule.QuikSimulator;
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
				while(finishedTrades.Count > 0)
					yield return finishedTrades.Dequeue();
			}
			finishedTradesBuilder.NewTradeFinished -= SetFinishedTrade;
			State = HystoryProcessorState.Finished;
			ProgressChanged?.Invoke(this);
		}
	}
	class FinishedTradesFromTicks : IEnumerable<FinishedTrade>
	{
		public float ProgressPromille { get => processedTicksCount * 1000 / totalTicksCount; }
		public HystoryProcessorState State = HystoryProcessorState.Created;
		IDecisionProvider decisionProvider;
		IEnumerable<Tick> ticks;
		int totalTicksCount = 1;
		int processedTicksCount;
		public FinishedTradesFromTicks(IEnumerable<Tick> ticks, IDecisionProvider decisionProvider)
		{
			this.ticks = ticks;
			this.decisionProvider = decisionProvider;
		}
		public IEnumerator<FinishedTrade> GetEnumerator()
		{
			if (State != HystoryProcessorState.Created) 
				throw new Exception("can't start two enumerators at the same time");
			State = HystoryProcessorState.Counting;
			totalTicksCount = ticks.Count();
			State = HystoryProcessorState.Ready;

			decisionProvider.Clear();

			HystoryTradingModule tradingModule =
				new(
					decisionProvider.TradeWithStopProvider,
					decisionProvider.ClosingProvider);


			FinishedTradesProvider finishedTradesBuilder = new();
			tradingModule.TradeExecuted += finishedTradesBuilder.OnTradeOpened;
			tradingModule.TradeClosedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;
			tradingModule.StopExecutedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;


			FinishedTrade? finishedTrade = null;
			void SetFinishedTrade(FinishedTrade trade) => finishedTrade = trade;
			finishedTradesBuilder.NewTradeFinished += SetFinishedTrade;


			HystoryQuikSimulator quikSimulator = tradingModule.QuikSimulator;
			finishedTradesBuilder.NewTick += quikSimulator.OnNewTick;
			quikSimulator.NewTick += decisionProvider.OnNewTick;


			State = HystoryProcessorState.Processing;
			int count = 0;
			foreach (Tick tick in ticks)
			{
				finishedTradesBuilder.OnNewTick(tick);
				processedTicksCount++;
				count++;
				if (finishedTrade != null)
				{
					yield return (FinishedTrade)finishedTrade;
					finishedTrade = null;
				}
			}
			finishedTradesBuilder.NewTradeFinished -= SetFinishedTrade;
			State = HystoryProcessorState.Created;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
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
