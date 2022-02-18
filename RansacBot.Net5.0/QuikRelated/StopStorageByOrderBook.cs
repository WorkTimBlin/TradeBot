using QuikSharp.DataStructures;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.QuikRelated
{
	class StopStorageByOrderBook : ITradesHystory
	{
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;
		private readonly TradeParams tradeParams;

		readonly SortedList<double> longs = new(Comparer<double>.Create((a, b) =>  a > b ? 1 : -1 ));
		readonly SortedList<double> shorts = new(Comparer<double>.Create((a, b) => a > b ? -1 : 1 ));


		public StopStorageByOrderBook(IProviderByParam<OrderBook> provider, TradeParams param)
		{
			//provider.Subscribe(param);
		}

		public void OnNewOrderBook(OrderBook orderBook)
		{
			
		}

		public void ClosePercentOfLongs(double percent)
		{
			throw new NotImplementedException();
		}

		public void ClosePercentOfShorts(double percent)
		{
			throw new NotImplementedException();
		}

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			throw new NotImplementedException();
		}
	}
}
