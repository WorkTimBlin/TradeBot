using RansacBot.QuikRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	abstract class AbstractKilledStopsMarketCompensator<TOrder> : ITradeWithStopProcessor
	{
		Dictionary<AbstractOrderEnsurerWithPrice<TOrder>, TradeWithStop> marketOrders;

		public Action<TradeWithStop, double> FullyClosedTradeWithStop;

		public void OnNewTradeWithStop(TradeWithStop trade)
		{
			AbstractOrderEnsurerWithPrice<TOrder> ensurer = GetEnsurer(trade);
			ensurer.OrderEnsuranceStatusChanged += OnOrderEnsuranceStatusChanged;
			marketOrders.Add(ensurer, trade);
			ensurer.SubscribeSelfAndSendOrder();
		}

		public void OnOrderEnsuranceStatusChanged(object aEnsurer)
		{
			AbstractOrderEnsurerWithPrice<TOrder> ensurer = aEnsurer as AbstractOrderEnsurerWithPrice<TOrder> ?? 
				throw new Exception("can't cast aEnsurer to needed type");
			if (ensurer.IsComplete)
			{
				if(ensurer.State == EnsuranceState.Killed)
				{
					//wat
					throw new Exception("how can somebody kill MARKET ORDER?");
				}
				if(ensurer.State == EnsuranceState.Executed)
				{
					FullyClosedTradeWithStop?.Invoke(marketOrders[ensurer], ensurer.CompletionAttribute);
				}
				marketOrders.Remove(ensurer);
			}
		}

		protected abstract AbstractOrderEnsurerWithPrice<TOrder> GetEnsurer(TradeWithStop trade);
	}
}
