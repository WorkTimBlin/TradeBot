using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class FinishedTradesBuilder : ITickFilter
	{

		public event Action<FinishedTrade> NewTradeFinished;

		public event Action<Tick> NewTick;

		Dictionary<TradeWithStop, Tick> openingTicksOfTrades = new();
		Tick lastTick;

		public void OnNewTick(Tick tick)
		{
			lastTick = tick;
			NewTick.Invoke(tick);
		}

		public void OnTradeOpend(TradeWithStop tradeWithStop)
		{
			openingTicksOfTrades.Add(tradeWithStop, lastTick);
		}

		public void OnTradeClosedOnPrice(TradeWithStop tradeWithStop, double closingPrice)
		{
			NewTradeFinished?.Invoke(new(tradeWithStop, closingPrice, openingTicksOfTrades[tradeWithStop], lastTick));
		}
	}

	readonly struct FinishedTrade
	{
		public readonly Trade trade;
		public readonly double closingPrice;
		public readonly Tick openingTick;
		public readonly Tick closingTick;

		public FinishedTrade(Trade trade, double closingPrice, Tick openingTick, Tick closingTick)
		{
			this.trade = trade;
			this.closingPrice = closingPrice;
			this.openingTick = openingTick;
			this.closingTick = closingTick;
		}
	}
}
