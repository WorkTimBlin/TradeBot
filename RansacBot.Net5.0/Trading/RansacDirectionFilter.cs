using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class RansacDirectionFilter:ITradeFilter
	{
		int ransacLevel;
		RansacType currentRansac = RansacType.None;
		public event TradeHandler NewTrade;
		public void OnNewTrade(Trade trade)
		{
			if (
				currentRansac == RansacType.Falling && trade.direction == TradeDirection.sell ||
				currentRansac == RansacType.Raising && trade.direction == TradeDirection.buy)
				NewTrade?.Invoke(trade);
		}
		private void ChangeRansac(Ransac ransac)
		{
			if (ransac.Slope > 0)
			{
				currentRansac = RansacType.Raising;
			}
			else
			{
				currentRansac = RansacType.Falling;
			}
		}
		public void OnNewRansac(Ransac ransac, int level)
		{
			if (level != ransacLevel) return;
			ChangeRansac(ransac);
		}
		public void OnRebuildRansac(Ransac ransac, int level)
		{
			OnNewRansac(ransac, level);
		}
		public void OnStopRansac(Ransac ransac, int level)
		{
			if (level != ransacLevel) return;
			currentRansac = RansacType.None;
		}
		private enum RansacType
		{
			None,
			Raising, 
			Falling
		}
	}
}
