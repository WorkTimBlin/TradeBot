using QuikSharp.DataStructures;
using QuikSharp;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;

namespace RansacBot.QuikRelated
{
	class StopStorageClassic : ITradesHystory
	{
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;

		private readonly TradeParams tradeParams;
		Task timer;

		public readonly SortedList<StopOrderEnsurer> longs = 
			new(Comparer<StopOrderEnsurer>.Create(
				(x, y) => x.Order.ConditionPrice > y.Order.ConditionPrice ? 1 : -1));
		public readonly SortedList<StopOrderEnsurer> shorts = 
			new(Comparer<StopOrderEnsurer>.Create(
				(x, y) => x.Order.ConditionPrice > y.Order.ConditionPrice ? -1 : 1));

		public StopStorageClassic(TradeParams tradeParams)
		{
			this.tradeParams = tradeParams;
			LaunchTimer(150);
		}

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			StopOrder stopOrder = BuildStopOrder(tradeWithStop);
			if (IsLong(stopOrder)) longs.Add(new(stopOrder));
			else shorts.Add(new(stopOrder));
		}

		public void ClosePercentOfLongs(double percent)
		{
			KillPercentOfStops(percent, longs);
		}
		public void ClosePercentOfShorts(double percent)
		{
			KillPercentOfStops(percent, shorts);
		}

		private void KillPercentOfStops(double percent, SortedList<StopOrderEnsurer> orders)
		{
			for (int i = (int)(orders.Count * (100 - percent) / 100); i < orders.Count; i++)
			{
				if(orders[i].State == EnsuranceState.Active)
				{
					orders[i].Kill();
				}
			}
		}

		bool IsLong(StopOrder stopOrder)
		{
			return stopOrder.Operation == Operation.Sell;
		}

		void LaunchTimer(int milliseconds)
		{
			if (timer != null) throw new Exception("timer is already Launched");
			timer = Task.Run(() =>
			{
				while (true)
				{
					ClearCompletedStops();
					Task.Delay(milliseconds).Wait();
				}
			});
		}

		void ClearCompletedStops()
		{
			ProcessAndClearCompletedStopsIn(longs);
			ProcessAndClearCompletedStopsIn(shorts);
		}
		void ProcessAndClearCompletedStopsIn(SortedList<StopOrderEnsurer> stops)
		{
			for(int i = 0; i < stops.Count; i++)
			{
				if (stops[i].IsComplete)
				{
					if(stops[i].State == EnsuranceState.Killed)
					{
						CompensateKilledStopWithMarketOrder(stops[i].Order);
					}
					InvokeCompletionOfStop(stops[i].Order);
					stops.RemoveAt(i);
				}
			}
		}

		void OnStopOrderEnsuranceStatusChanged(AbstractOrderEnsurer<StopOrder> ensurer)
		{
			if (ensurer.IsComplete)
			{
				ensurer.OrderEnsuranceStatusChanged -= OnStopOrderEnsuranceStatusChanged;
				if(ensurer.State == EnsuranceState.Killed)
				{
					CompensateKilledStopWithMarketOrder(ensurer.Order);
				}
				InvokeCompletionOfStop(ensurer.Order);
				RemoveStopFromList(
					ensurer as StopOrderEnsurer ?? 
					throw new Exception("this OrderEnsurer is not StopOrderEnsurer"));
			}
		}

		void InvokeCompletionOfStop(StopOrder stopOrder)
		{
			(stopOrder.State == State.Completed ?
				(IsLong(stopOrder) ? ExecutedLongStop : ExecutedShortStop) :
				(IsLong(stopOrder) ? KilledLongStop : KilledShortStop)).Invoke(stopOrder.Price, stopOrder.Price);
		}
		void CompensateKilledStopWithMarketOrder(StopOrder stopOrder)
		{
			EnsureSendingMarketOrder(stopOrder.Operation, stopOrder.Quantity);
		}
		void EnsureSendingMarketOrder(Operation operation, int qty)
		{
			OrderEnsurer ensurer = new OrderEnsurer(QuikHelpFunctions.BuildMarketOrder(operation, tradeParams, qty));
			ensurer.SubscribeSelfAndSendOrder();
			if (new OrderEnsurer(QuikHelpFunctions.BuildMarketOrder(operation, tradeParams, qty)).Order.TransID < 0)
			{
				throw new Exception("couldn't send market order for " + operation.ToString());
			}
		}
		void RemoveStopFromList(StopOrderEnsurer ensurer)
		{
			(IsLong(ensurer.Order) ? longs : shorts).Remove(ensurer);
		}

		StopOrder BuildStopOrder(Trading.TradeWithStop trade)
		{
			return QuikHelpFunctions.BuildStopOrder(trade, tradeParams, 1);
		}

		public List<string> GetLongs()
		{
			return new(
				longs.Select(
					(StopOrderEnsurer stopOrder) => 
					{ return stopOrder.Order.TransId.ToString() + " " + stopOrder.Order.ConditionPrice.ToString(); }));
		}
		public List<string> GetShorts()
		{
			return new(
				shorts.Select(
					(StopOrderEnsurer stopOrder) => 
					{ return stopOrder.Order.TransId.ToString() + " " + stopOrder.Order.ConditionPrice.ToString(); }));
		}
	}


	/// <summary>
	/// needs AbstractOrderEnsurer that gives callback when got executionPrice
	/// </summary>
	/// <typeparam name="TOrder"></typeparam>
	abstract public class QuikStopsOperator<TOrder> : IStopsOperator
	{
		public event Action<TradeWithStop, double> StopExecuted;
		public event Action<TradeWithStop> UnexecutedStopRemoved;

		private readonly TradeParams tradeParams;

		readonly SortedDictionary<AbstractOrderEnsurer<TOrder>, TradeWithStop> longs;
		readonly SortedDictionary<AbstractOrderEnsurer<TOrder>, TradeWithStop> shorts;

		public QuikStopsOperator(
			Comparer<AbstractOrderEnsurer<TOrder>> longsComparer, 
			Comparer<AbstractOrderEnsurer<TOrder>> shortsComparer)
		{
			longs = new(longsComparer);
			shorts = new(shortsComparer);
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
			AbstractOrderEnsurer<TOrder> ensurer = BuildStopOrderEnsurer(tradeWithStop);
			ensurer.OrderEnsuranceStatusChanged += OnStopOrderEnsuranceStatusChanged;
			ensurer.SubscribeSelfAndSendOrder();
			AddToStops(ensurer, tradeWithStop);
		}

		void KillPercentOfStops(SortedDictionary<AbstractOrderEnsurer<TOrder>, TradeWithStop> stopsDic, double percent)
		{
			IEnumerable<AbstractOrderEnsurer<TOrder>> stops = stopsDic.Keys.Skip((int)(stopsDic.Count * (100 - percent) / 100));
			foreach(AbstractOrderEnsurer<TOrder> stop in stops)
			{
				if (stop.State == EnsuranceState.Active)
				{
					stop.Kill();
				}
			}
		}

		void OnStopOrderEnsuranceStatusChanged(AbstractOrderEnsurer<TOrder> ensurer)
		{
			if (ensurer.IsComplete)
			{
				if (ensurer.State == EnsuranceState.Killed)
				{
					UnexecutedStopRemoved?.Invoke(GetTradeWithStop(ensurer));
				}
				if(ensurer.State == EnsuranceState.Executed)
				{
					if (ensurer.ExecutionPrice == 0) return;
					StopExecuted?.Invoke(GetTradeWithStop(ensurer), ensurer.ExecutionPrice);
				}
				RemoveStopFromList(ensurer);
			}
		}

		void RemoveStopFromList(AbstractOrderEnsurer<TOrder> ensurer)
		{
			ensurer.OrderEnsuranceStatusChanged -= OnStopOrderEnsuranceStatusChanged;
			GetDict(ensurer).Remove(ensurer);
		}
		void AddToStops(AbstractOrderEnsurer<TOrder> ensurer, TradeWithStop tradeWithStop)
		{
			GetDict(ensurer).Add(ensurer, tradeWithStop);
		}
		TradeWithStop GetTradeWithStop(AbstractOrderEnsurer<TOrder> ensurer) =>
			GetDict(ensurer)[ensurer];
		protected abstract SortedDictionary<AbstractOrderEnsurer<TOrder>, TradeWithStop> 
			GetDict(AbstractOrderEnsurer<TOrder> ensurer);

		protected abstract AbstractOrderEnsurer<TOrder> BuildStopOrderEnsurer(Trading.TradeWithStop trade);

		public abstract List<string> GetLongs();
		public abstract List<string> GetShorts();
	}
}
