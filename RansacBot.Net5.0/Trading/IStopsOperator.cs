using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	public interface IStopsOperator : IStopsCloser
	{
		public event Action<TradeWithStop, double> ExecutedStopExecuted;
		public event Action<TradeWithStop> UnexecutedStopRemoved;

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop);
	}

	public interface IStopsCloser
	{
		public void ClosePercentOfLongs(double percent);
		public void ClosePercentOfShorts(double percent);
	}
}
