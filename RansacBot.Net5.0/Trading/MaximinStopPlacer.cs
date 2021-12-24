using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class MaximinStopPlacer:IStopPlacer
	{
		

		public event TradeWithStopHandler NewTradeWithStop;
		public void OnNewTrade(Trade trade)
		{
			
		}

	}
}
