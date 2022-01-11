using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.UI
{
    class RansacsOxyPrinterWithTradesDemo : RansacsOxyPrinterWithTrades
    {
		public RansacsOxyPrinterWithTradesDemo(int level, RansacsCascade cascade, bool firstOnly) : base(level, cascade)
		{
			if (firstOnly)
			{
				cascade.RebuildRansac -= OnRebuildRansac;
				cascade.StopRansac -= OnStopRansac;
			}
		}
	}
}
