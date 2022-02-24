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
		public event Action<TradeWithStop> TradeExecuted;
		public event Action<TradeWithStop, double> StopExecutedOnPrice;
		public event Action<TradeWithStop, double> TradeClosedOnPrice;
		public IStopsContainer StopsContainer { get => stopsOperator; }

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
		QuikKilledStopsMarketCompensator compensator;

		public QuikTradingModule(TradeParams tradeParams, 
			ITradeWithStopProvider tradeWithStopProvider, 
			IClosingProvider closingProvider) :
			this(tradeParams)
		{
			TradeWithStopProvider = tradeWithStopProvider;
			ClosingProvider = closingProvider;
		}

		public QuikTradingModule(TradeParams tradeParams)
		{
			this.tradeParams = tradeParams;
			checkpoint = new(tradeParams);
			stopsOperator = new(tradeParams);
			checkpoint.NewTradeWithStop += stopsOperator.OnNewTradeWithStop;
			compensator = new(tradeParams);
			stopsOperator.UnexecutedStopRemoved += compensator.OnNewTradeWithStop;

			checkpoint.NewTradeWithStop += InvokeTradeExecuted;
			stopsOperator.StopExecuted += InvokeTradeStopExecuted;
			compensator.FullyClosedTradeWithStop += InvokeTradeClosedOnPrice;
		}

		void InvokeTradeExecuted(TradeWithStop tradeWithStop) =>
			TradeExecuted?.Invoke(tradeWithStop);
		void InvokeTradeStopExecuted(TradeWithStop tradeWithStop, double executionPrice) =>
			StopExecutedOnPrice?.Invoke(tradeWithStop, executionPrice);
		void InvokeTradeClosedOnPrice(TradeWithStop tradeWithStop, double executionPrice) =>
			TradeClosedOnPrice?.Invoke(tradeWithStop, executionPrice);
	}
}
