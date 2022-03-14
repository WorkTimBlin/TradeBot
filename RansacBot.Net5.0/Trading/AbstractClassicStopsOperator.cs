using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	abstract class AbstractClassicStopsOperator<TStopOrder, TOrder> : IStopsOperator, IStopsContainer
	{
		public event Action<TradeWithStop, double> ExecutedStopExecuted;
		public event Action<TradeWithStop> UnexecutedStopRemoved;

		public bool CloseSlippedWithBoth { get; set; }

		readonly AbstractSortedOrdersWaitroom<TStopOrder, TOrder> longs;
		readonly AbstractSortedOrdersWaitroom<TStopOrder, TOrder> shorts;

		readonly AbstractSentOrdersWaitroom<TOrder, double> executed;

		public AbstractClassicStopsOperator(
			Comparison<AbstractOrderEnsurerWithCompletionAttribute<TStopOrder, TOrder>> longsComparer,
			Comparison<AbstractOrderEnsurerWithCompletionAttribute<TStopOrder, TOrder>> shortsComparer,
			bool closeSlippedWithBoth = true)
		{
			longs = GetNewWaitroom(longsComparer);
			shorts = GetNewWaitroom(shortsComparer);
			executed = GetNewExecutedWaitroom();
			longs.OrderKilled += OnUnexecutedStopRemoved;
			longs.OrderExecuted += OnStopExecuted;
			shorts.OrderKilled += OnUnexecutedStopRemoved;
			shorts.OrderExecuted += OnStopExecuted;
			executed.OrderExecuted += OnExecutedStopExecuted;
			executed.OrderKilled += OnUnexecutedStopRemoved;
			this.CloseSlippedWithBoth = closeSlippedWithBoth;
		}

		protected abstract AbstractSentOrdersWaitroom<TOrder, double> GetNewExecutedWaitroom();

		public void ClosePercentOfLongs(double percent)
		{
			if (CloseSlippedWithBoth) executed.KillLastPercent(100);
			longs.KillLastPercent(percent);
		}
		public void ClosePercentOfShorts(double percent)
		{
			if(CloseSlippedWithBoth) executed.KillLastPercent(100);
			shorts.KillLastPercent(percent);
		}
		public void ClosePercentOfSlippedStops(double percent)
		{
			executed.KillLastPercent(percent);
		}
		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			TStopOrder stop = BuildStopOrder(tradeWithStop);
			GetWaitroom(stop).OnNewOrder(tradeWithStop, stop);
		}

		private void OnUnexecutedStopRemoved(TradeWithStop tradeWithStop) => UnexecutedStopRemoved?.Invoke(tradeWithStop);
		private void OnStopExecuted(TradeWithStop tradeWithStop, TOrder order)
		{
			executed.OnNewSentOrder(tradeWithStop, order);
		}
		private void OnExecutedStopExecuted(TradeWithStop tradeWithStop, double price)
		{
			ExecutedStopExecuted?.Invoke(tradeWithStop, price);
		}
		private AbstractSortedOrdersWaitroom<TStopOrder, TOrder> GetWaitroom(TStopOrder stopOrder) => IsLong(stopOrder) ? longs : shorts;
		protected abstract TStopOrder BuildStopOrder(TradeWithStop tradeWithStop);
		protected abstract bool IsLong(TStopOrder stopOrder);
		protected abstract AbstractSortedOrdersWaitroom<TStopOrder, TOrder> 
			GetNewWaitroom(Comparison<AbstractOrderEnsurerWithCompletionAttribute<TStopOrder, TOrder>> comparison);

		public IEnumerable<string> GetLongs()
		{
			return longs.GetAsStrings();
		}
		public IEnumerable<string> GetShorts()
		{
			return shorts.GetAsStrings();
		}
		public IEnumerable<string> GetExecuted()
		{
			return executed.GetAsStrings();
		}
	}
}
