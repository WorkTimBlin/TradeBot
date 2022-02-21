using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	abstract class AbstractOneAtATimeCheckpoint<TOrder> : ITradeWithStopFilter
	{
		public event TradeWithStopHandler NewTradeWithStop;

		AbstractOrderEnsurer<TOrder> orderEnsurer;

		TradeWithStop currentTradeWithStop;

		Task? killingAttempt = null;

		bool goodToGo = true;


		public void OnNewTradeWithStop(TradeWithStop trade)
		{
			if (goodToGo)
			{
				goodToGo = false;
				currentTradeWithStop = trade;
				orderEnsurer = GetNewOrderEnsurer(currentTradeWithStop);
				Task.Run(WaitForCurrentToCompleteAndInvoke);
			}
			else
			{
				if (killingAttempt == null)
				{
					killingAttempt = Task.Run(() =>
					{
						WaitForArrival();
						if (orderEnsurer.State == EnsuranceState.Active)
						{
							orderEnsurer.Kill();
						}
						killingAttempt = null;
					});
				}
			}
		}

		private void WaitForArrival()
		{
			Task ArrivalAwaiter = Task.Run(() =>
			{
				while (orderEnsurer.State == EnsuranceState.Sent) ;
			});
			do
			{
				orderEnsurer.UpdateOrderFromQuikByTransID();
			}
			while (!ArrivalAwaiter.Wait(10));
		}

		private void WaitForCurrentToCompleteAndInvoke()
		{
			//WaitForArrival(); // - can be used for awaiting order arrival to quik, not necessary now
			Task observingTimer = Task.Run(() => 
			{ 
				while (!orderEnsurer.IsComplete) ; 
			});
			while (!observingTimer.Wait(30))
			{
				orderEnsurer.UpdateOrderFromQuikByTransID();
			}
			if (orderEnsurer.State == EnsuranceState.Executed)
			{
				NewTradeWithStop?.Invoke(currentTradeWithStop);
			}
			goodToGo = true;
		}

		//protected abstract TOrderEnsurer GetNewOrderEnsurer<TOrderEnsurer>(Trading.Trade trade)
		//where TOrderEnsurer : AbstractOrderEnsurer<TOrder>;
		protected abstract AbstractOrderEnsurer<TOrder> GetNewOrderEnsurer(Trading.Trade trade);
	}
}
