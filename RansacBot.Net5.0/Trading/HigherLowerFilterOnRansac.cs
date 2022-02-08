using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class HigherLowerFilterOnRansac:ITradeFilter
	{
		double high;
		double low;
		public event TradeHandler NewTrade;

		readonly int level;


		public HigherLowerFilterOnRansac(RansacsCascade ransacsCascade, int level)
		{
			ransacsCascade.StopRansac += OnStopRansac;
			this.level = level;
			OnStopRansac(new(0, 0, 0, 0, 0, 0, 0, 0), level);
		}

		void OnStopRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			high = double.MinValue;
			low = double.MaxValue;
		}

		public void OnNewTrade(Trade trade)
		{
			if(trade.direction == TradeDirection.buy && trade.price > high)
			{
				high = trade.price;
				NewTrade.Invoke(trade);
			}
			if(trade.direction == TradeDirection.sell && trade.price < low)
			{
				low = trade.price;
				NewTrade.Invoke(trade);
			}
		}
	}
}
