using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	class FinishedTradesProvider : IFinishedTradeProvider
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
			openingTicksOfTrades.Remove(tradeWithStop);
		}

		public FinishedTrade PeekFinishedTradeFromClosed(TradeWithStop tradeWithStop, double closingPrice)
		{
			return new(tradeWithStop, closingPrice, openingTicksOfTrades[tradeWithStop], lastTick);
		}
	}

	interface IFinishedTradeProvider : ITickFilter
	{
		public event Action<FinishedTrade> NewTradeFinished;
		public void OnTradeClosedOnPrice(TradeWithStop tradeWithStop, double closingPrice);
	}

	readonly struct FinishedTrade
	{
		public readonly TradeWithStop trade;
		public readonly double closingPrice;
		public readonly Tick openingTick;
		public readonly Tick closingTick;

		public FinishedTrade(TradeWithStop trade, double closingPrice, Tick openingTick, Tick closingTick)
		{
			this.trade = trade;
			this.closingPrice = closingPrice;
			this.openingTick = openingTick;
			this.closingTick = closingTick;
		}

		public override string ToString()
		{
			return String.Join(';', SplittedString());
		}
		public IEnumerable<string> SplittedString()
		{
			yield return trade.direction == TradeDirection.buy ? "B" : "S";
			yield return trade.stop.price.ToString();
			yield return openingTick.ID.ToString();
			yield return closingTick.ID.ToString();
			yield return trade.price.ToString();
			yield return closingPrice.ToString();
			yield return "1";
		}
	}
}
