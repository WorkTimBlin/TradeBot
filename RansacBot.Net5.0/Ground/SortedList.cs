using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	class UpSortedList<T> : List<T>
	{
		public new void Add(T item)
		{
			int IndexForItem = BinarySearch(item);

			if (IndexForItem < 0) Insert(Math.Abs(IndexForItem) - 1, item);
			else Insert(IndexForItem, item);
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
			int IndexForItem = BinarySearch(item);

			if (IndexForItem < 0) Insert(Math.Abs(IndexForItem) - 1, item);
			else Insert(IndexForItem, item);
		}
	}
}
