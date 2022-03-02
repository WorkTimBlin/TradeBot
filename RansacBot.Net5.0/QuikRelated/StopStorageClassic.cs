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

		public readonly SortedList<QuikStopOrderEnsurer> longs = 
			new(Comparer<QuikStopOrderEnsurer>.Create(
				(x, y) => x.Order.ConditionPrice > y.Order.ConditionPrice ? 1 : -1));
		public readonly SortedList<QuikStopOrderEnsurer> shorts = 
			new(Comparer<QuikStopOrderEnsurer>.Create(
				(x, y) => x.Order.ConditionPrice > y.Order.ConditionPrice ? -1 : 1));

		public StopStorageClassic(TradeParams tradeParams)
		{
			this.tradeParams = tradeParams;
			LaunchTimer(500);
		}

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			StopOrder stopOrder = BuildStopOrder(tradeWithStop);
			QuikStopOrderEnsurer ensurer = new(stopOrder);
			ensurer.OrderEnsuranceStatusChanged += OnStopOrderEnsuranceStatusChanged;
			ensurer.SubscribeSelfAndSendOrder();
			if (IsLong(stopOrder)) longs.Add(ensurer);
			else shorts.Add(ensurer);
		}

		public void ClosePercentOfLongs(double percent)
		{
			KillPercentOfStops(percent, longs);
		}
		public void ClosePercentOfShorts(double percent)
		{
			KillPercentOfStops(percent, shorts);
		}

		private void KillPercentOfStops(double percent, SortedList<QuikStopOrderEnsurer> orders)
		{
			UpdateStopsFromQuik(orders);
			foreach(QuikStopOrderEnsurer ensurer in orders.Skip((int)(orders.Count * (100 - percent) / 100)))
			{
				if (ensurer.State == EnsuranceState.Active)
					ensurer.Kill();
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
					UpdateAllStopsFromQuik();
					Task.Delay(milliseconds).Wait();
				}
			});
		}

		void UpdateAllStopsFromQuik()
		{
			UpdateStopsFromQuik(longs);
			UpdateStopsFromQuik(shorts);
		}
		void UpdateStopsFromQuik(SortedList<QuikStopOrderEnsurer> stops)
		{
			foreach(QuikStopOrderEnsurer ensurer in stops)
			{
				ensurer.UpdateOrderFromQuikByTransID();
			}
		}



		void OnStopOrderEnsuranceStatusChanged(object aEnsurer)
		{
			QuikStopOrderEnsurer? ensurer = aEnsurer as QuikStopOrderEnsurer;
			if (ensurer == null) return;
			if (ensurer.IsComplete)
			{
				if (IsInList(ensurer as QuikStopOrderEnsurer))
				{
					ensurer.OrderEnsuranceStatusChanged -= OnStopOrderEnsuranceStatusChanged;
					if(ensurer.State == EnsuranceState.Killed)
					{
						CompensateKilledStopWithMarketOrder(ensurer.Order);
					}
					InvokeCompletionOfStop(ensurer.Order);
					RemoveStopFromList(ensurer);
				}
			}
		}

		void InvokeCompletionOfStop(StopOrder stopOrder)
		{
			(stopOrder.State == State.Completed ?
				(IsLong(stopOrder) ? ExecutedLongStop : ExecutedShortStop) :
				(IsLong(stopOrder) ? KilledLongStop : KilledShortStop)).Invoke((double)stopOrder.Price, (double)stopOrder.Price);
		}
		void CompensateKilledStopWithMarketOrder(StopOrder stopOrder)
		{
			EnsureSendingMarketOrder(stopOrder.Operation, stopOrder.Quantity);
		}
		void EnsureSendingMarketOrder(Operation operation, int qty)
		{
			QuikOrderEnsurer ensurer = new QuikOrderEnsurer(QuikHelpFunctions.BuildMarketOrder(operation, tradeParams, qty));
			ensurer.SubscribeSelfAndSendOrder();
			if (new QuikOrderEnsurer(QuikHelpFunctions.BuildMarketOrder(operation, tradeParams, qty)).Order.TransID < 0)
			{
				throw new Exception("couldn't send market order for " + operation.ToString());
			}
		}
		void RemoveStopFromList(QuikStopOrderEnsurer ensurer)
		{
			(IsLong(ensurer.Order) ? longs : shorts).Remove(ensurer);
		}
		bool IsInList(QuikStopOrderEnsurer ensurer) =>
			longs.Contains(ensurer) || shorts.Contains(ensurer);

		StopOrder BuildStopOrder(Trading.TradeWithStop trade)
		{
			return QuikHelpFunctions.BuildStopOrder(trade, tradeParams, 1);
		}

		public List<string> GetLongs()
		{
			return new(
				longs.Select(
					(QuikStopOrderEnsurer stopOrder) => 
					{ return stopOrder.Order.TransId.ToString() + " " + stopOrder.Order.ConditionPrice.ToString(); }));
		}
		public List<string> GetShorts()
		{
			return new(
				shorts.Select(
					(QuikStopOrderEnsurer stopOrder) => 
					{ return stopOrder.Order.TransId.ToString() + " " + stopOrder.Order.ConditionPrice.ToString(); }));
		}
	}
}
