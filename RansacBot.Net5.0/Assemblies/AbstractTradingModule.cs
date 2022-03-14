using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	abstract class AbstractTradingModule<TOrder, TStopOrder>
	{
		public event Action<TradeWithStop> TradeExecuted;
		public event Action<TradeWithStop, double> StopExecutedOnPrice;
		public event Action<TradeWithStop, double> TradeClosedOnPrice;
		public IStopsContainer StopsContainer { get => stopsOperator; }

		public ITradeWithStopProvider? TradeWithStopProvider
		{
			set
			{
				if (tradeWithStopProvider != null)
					tradeWithStopProvider.NewTradeWithStop -= checkpoint.OnNewTradeWithStop;
				tradeWithStopProvider = value;
				if (tradeWithStopProvider != null)
					tradeWithStopProvider.NewTradeWithStop += checkpoint.OnNewTradeWithStop;
			}
		}
		public IClosingProvider? ClosingProvider
		{
			set
			{
				if (closingProvider != null)
				{
					closingProvider.ClosePercentOfLongs -= stopsOperator.ClosePercentOfLongs;
					closingProvider.ClosePercentOfShorts -= stopsOperator.ClosePercentOfShorts;
				}
				closingProvider = value;
				if (closingProvider != null)
				{
					closingProvider.ClosePercentOfLongs += stopsOperator.ClosePercentOfLongs;
					closingProvider.ClosePercentOfShorts += stopsOperator.ClosePercentOfShorts;
				}
			}
		}

		public bool CompensateKilledStopWithMarketOrder
		{
			set
			{
				compensator = GetCompensator();
				stopsOperator.UnexecutedStopRemoved -= compensator.OnNewTradeWithStop;
				if (value) stopsOperator.UnexecutedStopRemoved += compensator.OnNewTradeWithStop;
			}
		}

		ITradeWithStopProvider? tradeWithStopProvider;
		IClosingProvider? closingProvider;

		AbstractOneAtATimeCheckpoint<TOrder> checkpoint;
		AbstractClassicStopsOperator<TStopOrder, TOrder> stopsOperator;
		AbstractMarketCompensator<TOrder> compensator;

		public AbstractTradingModule(
			ITradeWithStopProvider tradeWithStopProvider,
			IClosingProvider closingProvider) :
			this()
		{
			TradeWithStopProvider = tradeWithStopProvider;
			ClosingProvider = closingProvider;
		}

		public AbstractTradingModule()
		{
			checkpoint = GetCheckpoint();
			stopsOperator = GetStopsOperator();
			checkpoint.NewTradeWithStop += stopsOperator.OnNewTradeWithStop;
			compensator = GetCompensator();
			CompensateKilledStopWithMarketOrder = true;

			checkpoint.NewTradeWithStop += InvokeTradeExecuted;
			stopsOperator.ExecutedStopExecuted += InvokeTradeStopExecuted;
			compensator.FullyClosedTradeWithStop += InvokeTradeClosedOnPrice;
		}

		protected abstract AbstractOneAtATimeCheckpoint<TOrder> GetCheckpoint();
		protected abstract AbstractClassicStopsOperator<TStopOrder, TOrder> GetStopsOperator();
		protected abstract AbstractMarketCompensator<TOrder> GetCompensator();

		void InvokeTradeExecuted(TradeWithStop tradeWithStop) =>
			TradeExecuted?.Invoke(tradeWithStop);
		void InvokeTradeStopExecuted(TradeWithStop tradeWithStop, double executionPrice) =>
			StopExecutedOnPrice?.Invoke(tradeWithStop, executionPrice);
		void InvokeTradeClosedOnPrice(TradeWithStop tradeWithStop, double executionPrice) =>
			TradeClosedOnPrice?.Invoke(tradeWithStop, executionPrice);
	}
}
