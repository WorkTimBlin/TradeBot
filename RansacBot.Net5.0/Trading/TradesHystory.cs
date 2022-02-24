using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures;
using RansacBot.Trading;


namespace RansacBot.Trading
{
	public interface ITradesHystory
	{
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;
		public void ClosePercentOfLongs(double percent);
		public void ClosePercentOfShorts(double percent);

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop);
	}
	
	public delegate void ClosePosHandler(double closedStop, double executionPrice);
	class TradesHystory : ITradesHystory, ITradeWithStopFilter
	{
		public event Action<TradeWithStop> NewTradeWithStop;
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;

		UpSortedList<double> longStops = new();
		SortedList<double> shortStops = new(new DoubleComparer());

		public void OnNewTradeWithStop(TradeWithStop trade)
		{
			if (trade.direction == TradeDirection.buy)
			{
				longStops.Add(trade.stop.price);
			}
			else if (trade.direction == TradeDirection.sell)
			{
				shortStops.Add(trade.stop.price);
			}
			NewTradeWithStop?.Invoke(trade);
		}

		public void ClosePercentOfLongs(double percent)
		{
			int IndexToRemoveFrom = (int)(longStops.Count * (100 - percent) / 100);
			foreach (double price in longStops.GetRange(IndexToRemoveFrom, longStops.Count - IndexToRemoveFrom))
			{
				KilledLongStop?.Invoke(price, price);
			}
			longStops.RemoveRange(IndexToRemoveFrom, longStops.Count - IndexToRemoveFrom);
		}
		public void ClosePercentOfShorts(double percent)
		{
			int IndexToRemoveFrom = (int)(shortStops.Count * (100 - percent) / 100);
			foreach (double price in shortStops.GetRange(IndexToRemoveFrom, shortStops.Count - IndexToRemoveFrom))
			{
				KilledShortStop?.Invoke(price, price);
			}
			shortStops.RemoveRange(IndexToRemoveFrom, shortStops.Count - IndexToRemoveFrom);
		}

		public void CheckForStops(double price)
		{
			if (longStops.Count > 0 && longStops[^1] > price)
			{
				int i = longStops.Count;
				do
				{
					i--;
				}
				while (i >= 0 && longStops[i] >= price);
				i++;
				foreach (double stopPrice in longStops.GetRange(i, longStops.Count - i))
				{
					ExecutedLongStop?.Invoke(stopPrice, price);
				}
				longStops.RemoveRange(i, longStops.Count - i);
			}
			else if (shortStops.Count > 0 && shortStops[^1] < price)
			{
				int i = shortStops.Count;
				do
				{
					i--;
				}
				while (i >= 0 && shortStops[i] <= price);
				i++;
				foreach (double stopPrice in shortStops.GetRange(i, shortStops.Count - i))
				{
					ExecutedShortStop?.Invoke(stopPrice, price);
				}
				shortStops.RemoveRange(i, shortStops.Count - i);
			}
		}

	}
	class DoubleComparer : IComparer<double>
	{
		public int Compare(double first, double second)
		{
			if (first > second) return -1;
			else if (first == second) return 0;
			else return 1;
		}
	}
}
