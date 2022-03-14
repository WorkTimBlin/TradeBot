using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Ground
{
	abstract class MultipleFilter<TIFilter, TItem> : IList<TIFilter> where TIFilter : IItemFilter<TItem>
	{

		List<TIFilter> filters = new();


		public TIFilter this[int index]
		{
			get => filters[index];
			set
			{
				RemoveAt(index);
				Insert(index, value);
			}
		}

		public int Count => filters.Count;

		public bool IsReadOnly => false;

		public void Add(TIFilter item)
		{
			if (filters.Count > 0)
				UnsubscribeAtIndex(filters.Count - 1);
			filters.Add(item);
			if(Count > 1)
				SubscribeAtIndex(filters.Count - 2);
			SubscribeAtIndex(filters.Count - 1);
		}

		public void Clear()
		{
			if (filters.Count > 0)
				filters[^1].Unsubscribe(ThrowNewItem);
			filters.Clear();
		}

		public bool Contains(TIFilter item)
		{
			return filters.Contains(item);
		}

		public void CopyTo(TIFilter[] array, int arrayIndex)
		{
			filters.CopyTo(array, arrayIndex);
		}

		public IEnumerator<TIFilter> GetEnumerator()
		{
			return filters.GetEnumerator();
		}

		public int IndexOf(TIFilter item)
		{
			return filters.IndexOf(item);
		}

		public void Insert(int index, TIFilter item)
		{
			UnsubscribeAtIndex(index);
			filters.Insert(index, item);
			SubscribeAtIndex(index);
			if (index < filters.Count - 1)
				SubscribeAtIndex(index + 1);
		}

		public bool Remove(TIFilter item)
		{
			int index = filters.IndexOf(item);
			if (index < 0) return false;
			RemoveAt(index);
			return true;
		}

		public void RemoveAt(int index)
		{
			UnsubscribeAtIndex(index);
			filters.RemoveAt(index);
			SubscribeAtIndex(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return filters.GetEnumerator();
		}

		public void OnNewItem(TItem item)
		{
			if (filters.Count > 0) filters[0].Processor.Invoke(item);
			else ThrowNewItem(item);
		}

		protected abstract void ThrowNewItem(TItem item);
		private void UnsubscribeAtIndex(int index)
		{
			TIFilter item = filters[index];
			if (index > 0)
				filters[index - 1].Unsubscribe(item.Processor);
			item.Unsubscribe(index < filters.Count - 1 ?
				filters[index + 1].Processor :
				ThrowNewItem);
		}
		private void SubscribeAtIndex(int index)
		{
			UnsubscribeAtIndex(index);
			if (index > 0)
				filters[index - 1].Subscribe(filters[index].Processor);
			filters[index].Subscribe(index < filters.Count - 1 ?
				filters[index + 1].Processor :
				ThrowNewItem );
		}
	}
}
