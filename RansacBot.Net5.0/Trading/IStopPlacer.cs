using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	public delegate void TradeWithStopHandler(TradeWithStop trade);
	public interface IStopPlacer
	{
		void OnNewTrade(Trade trade);
		event TradeWithStopHandler NewTradeWithStop;
	}
}
