using RansacBot.QuikRelated;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	class QuikTradingModule
	{
		public ITradeWithStopProvider TradeWithStopProvider
		{
			set
			{
				if(tradeWithStopProvider != null)
					tradeWithStopProvider.NewTradeWithStop -= checkpoint.OnNewTradeWithStop;
				tradeWithStopProvider = value;
				tradeWithStopProvider.NewTradeWithStop += checkpoint.OnNewTradeWithStop;
			}
		}
		public IClosingProvider ClosingProvider
		{
			set
			{
				if(closingProvider != null)
				{
					closingProvider.ClosePercentOfLongs -= stopsOperator.ClosePercentOfLongs;
					closingProvider.ClosePercentOfShorts -= stopsOperator.ClosePercentOfShorts;
				}
				closingProvider = value;
				closingProvider.ClosePercentOfLongs += stopsOperator.ClosePercentOfLongs;
				closingProvider.ClosePercentOfShorts += stopsOperator.ClosePercentOfShorts;
			}
		}

		ITradeWithStopProvider tradeWithStopProvider;
		IClosingProvider closingProvider;

		readonly TradeParams tradeParams;
		OneOrderAtATimeCheckpoint checkpoint;
		QuikClassicStopsOperator stopsOperator;

		public QuikTradingModule(TradeParams tradeParams)
		{
			this.tradeParams = tradeParams;
			checkpoint = new(tradeParams);
			stopsOperator = new(tradeParams);
			checkpoint.NewTradeWithStop += stopsOperator.OnNewTradeWithStop;
		}
	}
}
