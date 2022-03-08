using RansacBot.Ground;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Filters
{
	class LinearTradeFilter : MultipleFilter<ITradeFilter, Trade>, IList<ITradeFilter>, ITradeFilter
	{

		public event Action<Trade> NewTrade;
		public void OnNewTrade(Trade trade)
		{
			OnNewItem(trade);
		}

		protected override void ThrowNewItem(Trade item)
		{
			NewTrade?.Invoke(item);
		}
	}
}
