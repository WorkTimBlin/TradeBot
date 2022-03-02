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

		readonly SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop> longs;
		readonly SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop> shorts;

		readonly Dictionary<AbstractOrderEnsurerWithPrice<TOrder>, TradeWithStop>
			executedStopsUnexecutedOrders = new();

		public AbstractClassicStopsOperator(
			Comparison<AbstractStopOrderEnsurer<TStopOrder, TOrder>> longsComparer,
			Comparison<AbstractStopOrderEnsurer<TStopOrder, TOrder>> shortsComparer)
		{
			longs = new(Comparer<AbstractStopOrderEnsurer<TStopOrder, TOrder>>.Create(
				(first, second) => first.IsSame(second.Order) ? 0 : longsComparer(first, second)));
			shorts = new(Comparer<AbstractStopOrderEnsurer<TStopOrder, TOrder>>.Create(
				(first, second) => first.IsSame(second.Order) ? 0 : shortsComparer(first, second)));
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
				stopsDic.Keys.Skip((int)(stopsDic.Count * (100 - percent) / 100)).ToList();
			foreach (AbstractStopOrderEnsurer<TStopOrder, TOrder> stop in stops)
			{
				stop.UpdateOrderFromQuikByTransID();
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
					order.Subscribe();
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
				if (ensurer.State == EnsuranceState.Killed)
				{
					UnexecutedStopRemoved?.Invoke(GetTradeWithStop(ensurer));
				}
				if (ensurer.State == EnsuranceState.Executed)
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
		TradeWithStop GetTradeWithStop(AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer)
		{
			try
			{
				return GetDict(ensurer)[ensurer];
			}
			catch
			{
				return GetDict(ensurer).Values.ToList()
					[GetDict(ensurer).Keys.ToList().FindIndex((ens) => ens == ensurer)];
			}
		}
		TradeWithStop GetTradeWithStop(AbstractOrderEnsurerWithPrice<TOrder> ensurer) =>
			executedStopsUnexecutedOrders[ensurer];


		private SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop>
			GetDict(AbstractStopOrderEnsurer<TStopOrder, TOrder> ensurer)
		{
			return IsLong(ensurer) ? longs : shorts;
		}
		protected abstract bool IsLong(AbstractStopOrderEnsurer<TStopOrder, TOrder> stopEnsurer);
		protected abstract AbstractStopOrderEnsurer<TStopOrder, TOrder> 
			BuildStopOrderEnsurer(Trading.TradeWithStop trade);
		protected abstract AbstractOrderEnsurerWithPrice<TOrder> GetOrderEnsurer(TOrder order);

		public IEnumerable<string> GetLongs()
		{
			return GetStopsStrings(longs);
		}
		public IEnumerable<string> GetShorts()
		{
			return GetStopsStrings(shorts);
		}
		public IEnumerable<string> GetExecuted()
		{
			return executedStopsUnexecutedOrders.Keys.Select(GetSerialized);
		}

		private IEnumerable<string> GetStopsStrings(
			SortedDictionary<AbstractStopOrderEnsurer<TStopOrder, TOrder>, TradeWithStop> stops)
		{
			return stops.Keys.Select(GetSerialized);
		}
		public abstract string GetSerialized(AbstractStopOrderEnsurer<TStopOrder, TOrder> stopOrder);
		public abstract string GetSerialized(AbstractOrderEnsurerWithPrice<TOrder> ensurer);
	}
}
