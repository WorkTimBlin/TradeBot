using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RansacBot.Trading
{
	public delegate void ClosePosHandler(List<double> closedStops);
	class TradesHystory
	{
		public event ClosePosHandler ExecuteLongStops;
		public event ClosePosHandler ExecuteShortStops;
		public event ClosePosHandler CloseLongs;
		public event ClosePosHandler CloseShorts;

		UpSortedList<double> longStops = new();
		SortedList<double> shortStops = new(new DoubleComparer());

		public void OnNewTrade(TradeWithStop trade)
		{
			if(trade.direction == TradeDirection.buy)
			{
				longStops.Add(trade.stop.price);
			}
			else if(trade.direction == TradeDirection.sell)
			{
				shortStops.Add(trade.stop.price);
			}
		}

		public void ClosePercentOfLongs(double percent)
		{
			int IndexToRemoveFrom = (int)(longStops.Count * (100 - percent) / 100);
			CloseLongs?.Invoke(longStops.GetRange(IndexToRemoveFrom, longStops.Count - IndexToRemoveFrom));
			longStops.RemoveRange(IndexToRemoveFrom, longStops.Count - IndexToRemoveFrom);
		}
		public void ClosePercentOfShorts(double percent)
		{
			int IndexToRemoveFrom = (int)(shortStops.Count * (100 - percent) / 100);
			CloseShorts?.Invoke(shortStops.GetRange(IndexToRemoveFrom, shortStops.Count - IndexToRemoveFrom));
			shortStops.RemoveRange(IndexToRemoveFrom, shortStops.Count - IndexToRemoveFrom);
		}

		public void CheckForStops(double price)
		{
			if(longStops[^1] > price)
			{
				int i = longStops.Count;
				do
				{
					i--;
				}
				while (longStops[i] >= price);
				i++;
				ExecuteLongStops?.Invoke(longStops.GetRange(i, longStops.Count - i));
				longStops.RemoveRange(i, longStops.Count - i);
			}
			else if (shortStops[^1] > price)
			{
				int i = shortStops.Count;
				do
				{
					i--;
				}
				while (shortStops[i] >= price);
				i++;
				ExecuteShortStops?.Invoke(shortStops.GetRange(i, shortStops.Count - i));
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
	class UpSortedList<T>:List<T>
	{
		public new void Add(T item)
		{
			Insert(Math.Abs(BinarySearch(item)) - 1, item);
		}
	}
	class SortedList<T> : List<T>
	{
		readonly IComparer<T> comparer;
		public SortedList(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}
		public new int BinarySearch(T Item)
		{
			return base.BinarySearch(Item, comparer);
		}
		public new void Add(T item)
		{
			Insert(Math.Abs(base.BinarySearch(item)) - 1, item);
		}
	}
}
