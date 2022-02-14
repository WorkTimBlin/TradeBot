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
	class StopStorage : ITradesHystory
	{
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;

		private readonly TradeParams tradeParams;
		private readonly Quik quik;

		SortedList<StopOrder> longStops = 
			new(
				new StopOrderComparer(
					(first, second) => first.ConditionPrice > second.ConditionPrice ? 1 : -1));
		SortedList<StopOrder> shortStops = 
			new(
				new StopOrderComparer(
					(first, second) => first.ConditionPrice < second.ConditionPrice ? 1 : -1));

		public StopStorage(Quik quik, TradeParams tradeParams)
		{
			this.quik = quik;
			this.tradeParams = tradeParams;
			quik.Events.OnStopOrder += OnStopOrderChanged;
		}
		public void OnNewStopOrder(StopOrder stopOrder)
		{
			if (stopOrder.IsLong()) longStops.Add(stopOrder);
			else shortStops.Add(stopOrder);
		}
		public void OnStopOrderChanged(StopOrder stopOrder)
		{
			SortedList<StopOrder> currentList = stopOrder.IsLong() ? longStops : shortStops;
			int indexOfOrder = currentList.FindIndex(
				(StopOrder stopOrderInList) => 
				{ return stopOrder.TransId == stopOrderInList.TransId; }
			);
			if(indexOfOrder > -1)
			{
				currentList[indexOfOrder] = stopOrder;
				if (stopOrder.State == State.Completed || stopOrder.State == State.Canceled)
				{
					ClosePosHandler handler = 
						stopOrder.State == State.Completed?
							stopOrder.IsLong() ? ExecutedLongStop : ExecutedShortStop :
							stopOrder.IsLong() ? KilledLongStop : KilledShortStop;
					handler?.Invoke(stopOrder.ConditionPrice);
					currentList.RemoveAt(indexOfOrder);
				}
			}
		}
		public void ClosePercentOfLongs(double percent)
		{
			ClosePercentOfTrades(percent, longStops, KilledLongStop);
		}
		public void ClosePercentOfShorts(double percent)
		{
			ClosePercentOfTrades(percent, shortStops, KilledShortStop);
		}
		public List<string> GetLongs()
		{
			return new(longStops.Select((StopOrder stopOrder) => { return stopOrder.TransId.ToString() + " " + stopOrder.ConditionPrice.ToString(); }));
		}
		public List<string> GetShorts()
		{
			return new(shortStops.Select((StopOrder stopOrder) => { return stopOrder.TransId.ToString() + " " + stopOrder.ConditionPrice.ToString(); }));
		}
		void ClosePercentOfTrades(double percent, SortedList<StopOrder> stopOrders, ClosePosHandler KillHandler)
		{
			for (int i = stopOrders.Count - 1; i > (int)(stopOrders.Count * (100 - percent) / 100) - 1; i--)
			{
				EnsureSendingMarketOrder(stopOrders[i].Operation);
			}
			KillLastPercent(percent, stopOrders);
		}
		void KillLastPercent(double percent, SortedList<StopOrder> orders)
		{
			for (int i = (int)(orders.Count * (100 - percent) / 100); i < orders.Count; i++)
			{
				quik.StopOrders.KillStopOrder(orders[i]).ConfigureAwait(false);
			}
		}
		void RemoveStopByTransId(StopOrder stopOrder, SortedList<StopOrder> stopOrders)
		{
			stopOrders.RemoveAt(
				stopOrders.FindIndex(
					(StopOrder match) => { return (match.TransId == stopOrder.TransId); }
					)
				);
		}
		void EnsureSendingMarketOrder(Operation operation)
		{
			if(
				quik.Orders.SendMarketOrder(
					tradeParams.classCode, 
					tradeParams.secCode, 
					tradeParams.accountId, 
					operation, 
					1).Result.TransID < 0)
			{
				throw new Exception("couldn't send market order for " + operation.ToString());
			}
		}
		private class StopOrderComparer : IComparer<StopOrder>
		{
			private Func<StopOrder, StopOrder, int> compare;
			public StopOrderComparer(Func<StopOrder, StopOrder, int> compare)
			{
				this.compare = compare;
			}

			public int Compare(StopOrder first, StopOrder second)
			{
				return compare(first, second);
			}
		}
	}
}
