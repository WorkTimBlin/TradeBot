using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class CloserOnRansacStops
	{
		readonly TradesHystory tradesHystory;
		readonly int level;
		readonly double percent;
		public CloserOnRansacStops(TradesHystory tradesHystory, RansacsCascade cascade, int level, double percent)
		{
			this.tradesHystory = tradesHystory;
			this.level = level;
			this.percent = percent;
			cascade.StopRansac += OnStopRansac;
		}

		private void OnStopRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			if(ransac.Slope > 0)
			{
				tradesHystory.ClosePercentOfLongs(percent);
			}
			else
			{
				tradesHystory.ClosePercentOfShorts(percent);
			}
		}
	}
}
