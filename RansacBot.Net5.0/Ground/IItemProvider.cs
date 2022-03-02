using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Ground
{
	public interface IItemProvider<TItem>
	{
		public event Action<TItem> NewItem;
		abstract void Subscribe(Action<TItem> action);
		abstract void Unsubscribe(Action<TItem> action);
		//abstract protected void Invoke(TItem item);
	}
	public interface IItemProcessor<TItem>
	{
		abstract Action<TItem> Processor { get; }
	}
	public interface IItemFilter<TItem> : IItemProcessor<TItem>, IItemProvider<TItem> { }
}
