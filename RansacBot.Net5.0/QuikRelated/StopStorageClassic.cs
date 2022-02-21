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
			if (stopOrder.IsLong()) longs.Add(new(stopOrder));
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

		bool IsLong(StopOrderEnsurer ensurer)
		{
			return ensurer.Order.IsLong();
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
		void InvokeCompletionOfStop(StopOrder stopOrder)
		{
			(stopOrder.State == State.Completed ?
				(stopOrder.IsLong() ? ExecutedLongStop : ExecutedShortStop) :
				(stopOrder.IsLong() ? KilledLongStop : KilledShortStop)).Invoke(stopOrder.Price);
		}
		void CompensateKilledStopWithMarketOrder(StopOrder stopOrder)
		{
			EnsureSendingMarketOrder(stopOrder.Operation, stopOrder.Quantity);
		}
		void EnsureSendingMarketOrder(Operation operation, int qty)
		{
			if (new OrderEnsurer(QuikHelpFunctions.BuildMarketOrder(operation, tradeParams, qty)).Order.TransID < 0)
			{
				throw new Exception("couldn't send market order for " + operation.ToString());
			}
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
}
