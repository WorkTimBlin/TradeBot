using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	/// <summary>
	/// needs AbstractOrderEnsurer that gives callback when got executionPrice
	/// </summary>
	/// <typeparam name="TStopOrder"></typeparam>
	abstract class AbstractClassicStopsOperator<TStopOrder, TOrder> : IStopsOperator, IStopsContainer
	{
		public event Action<TradeWithStop, double> StopExecuted;
		public event Action<TradeWithStop> UnexecutedStopRemoved;

		readonly protected SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop> longs;
		readonly protected SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop> shorts;

		readonly protected Dictionary<AbstractOrderEnsurerWithPrice<TOrder>, TradeWithStop> 
			executedStopsUnexecutedOrders = new();

		public AbstractClassicStopsOperator(
			Comparison<AbstractStopOrderEnsurer<TStopOrder, TOrder>> longsComparer,
			Comparison<AbstractStopOrderEnsurer<TStopOrder, TOrder>> shortsComparer)
		{
			longs = new(Comparer<AbstractStopOrderEnsurer<TStopOrder, TOrder>>.Create(longsComparer));
			shorts = new(Comparer<AbstractStopOrderEnsurer<TStopOrder, TOrder>>.Create(shortsComparer));
		}

		public void ClosePercentOfLongs(double percent)
		{
			KillPercentOfStops(longs, percent);
		}
		public void ClosePercentOfShorts(double percent)
		{
			KillPercentOfStops(shorts, percent);
		}
		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer = BuildStopOrderEnsurer(tradeWithStop);
			ensurer.OrderEnsuranceStatusChanged += OnStopOrderEnsuranceStatusChanged;
			ensurer.SubscribeSelfAndSendOrder();
			AddToStops(ensurer, tradeWithStop);
		}

		void KillPercentOfStops(
			SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop> stopsDic, 
			double percent)
		{
			IEnumerable<AbstractStopOrderEnsurer<TStopOrder, TOrder>> stops = 
				stopsDic.Keys.Skip((int)(stopsDic.Count * (100 - percent) / 100));
			foreach (AbstractStopOrderEnsurer<TStopOrder, TOrder> stop in stops)
			{
				if (stop.State == EnsuranceState.Active)
				{
					stop.Kill();
				}
			}
		}


		void OnStopOrderEnsuranceStatusChanged(object ensurer) =>
			OnStopOrderEnsuranceStatusChanged(ensurer as AbstractStopOrderEnsurer<TStopOrder, TOrder> ??
				throw new TypeAccessException("object was not of right type"));
		void OnStopOrderEnsuranceStatusChanged(AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer)
		{
			if (ensurer.IsComplete)
			{
				ensurer.OrderEnsuranceStatusChanged -= OnStopOrderEnsuranceStatusChanged;
				if (ensurer.State == EnsuranceState.Killed)
				{
					UnexecutedStopRemoved?.Invoke(GetTradeWithStop(ensurer));
				}
				if (ensurer.State == EnsuranceState.Executed)
				{
					AbstractOrderEnsurerWithPrice<TOrder> order = GetOrderEnsurer(ensurer.CompletionAttribute);
					order.OrderEnsuranceStatusChanged += OnExecutedStopOrderEnsuranceStatusChanged;
					executedStopsUnexecutedOrders.Add(order, GetTradeWithStop(ensurer));

					if (order.IsComplete) OnExecutedStopOrderEnsuranceStatusChanged(order);
					else order.UpdateOrderFromQuikByTransID();
				}
				RemoveStopFromList(ensurer);
			}
		}

		void OnExecutedStopOrderEnsuranceStatusChanged(object aEnsurer)
		{
			AbstractOrderEnsurerWithPrice<TOrder> ensurer = aEnsurer as AbstractOrderEnsurerWithPrice<TOrder> ??
				throw new TypeAccessException("can't cast aEnsurer to the right type");
			if (ensurer.IsComplete)
			{
				ensurer.OrderEnsuranceStatusChanged -= OnExecutedStopOrderEnsuranceStatusChanged;
				if(ensurer.State == EnsuranceState.Killed)
				{
					UnexecutedStopRemoved?.Invoke(GetTradeWithStop(ensurer));
				}
				if(ensurer.State == EnsuranceState.Executed)
				{
					StopExecuted?.Invoke(GetTradeWithStop(ensurer), ensurer.CompletionAttribute);
				}
				executedStopsUnexecutedOrders.Remove(ensurer);
			}
		}

		void RemoveStopFromList(AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer)
		{
			ensurer.OrderEnsuranceStatusChanged -= OnStopOrderEnsuranceStatusChanged;
			GetDict(ensurer).Remove(ensurer);
		}
		void AddToStops(AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer, TradeWithStop tradeWithStop)
		{
			GetDict(ensurer).Add(ensurer, tradeWithStop);
		}
		TradeWithStop GetTradeWithStop(AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer) =>
			GetDict(ensurer)[ensurer];
		TradeWithStop GetTradeWithStop(AbstractOrderEnsurerWithPrice<TOrder> ensurer) =>
			executedStopsUnexecutedOrders[ensurer];


		protected abstract SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop>
			GetDict(AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer);
		protected abstract AbstractStopOrderEnsurer<TStopOrder, TOrder> 
			BuildStopOrderEnsurer(Trading.TradeWithStop trade);
		protected abstract AbstractOrderEnsurerWithPrice<TOrder> GetOrderEnsurer(TOrder order);

		public abstract List<string> GetLongs();
		public abstract List<string> GetShorts();
	}
}
