using QuikSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacBot.Trading;
using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;

namespace RansacBot.QuikRelated
{
	class OneOrderAtATimeCheckpoint : ITradeWithStopFilter
	{
		public event TradeWithStopHandler NewTradeWithStop;

		TradeParams tradeParams;
		ITradesHystory stopStorage;

		OrderEnsurer orderEnsurer;

		TradeWithStop currentTradeWithStop;

		Task? killingAttempt = null;

		bool goodToGo = true;

		public OneOrderAtATimeCheckpoint (TradeParams tradeParams, StopStorageClassic stopStorage)
		{
			this.tradeParams = tradeParams;
			this.stopStorage = stopStorage;
		}


		public void OnNewTradeWithStop(TradeWithStop trade)
		{
			if (goodToGo)
			{
				goodToGo = false;
				currentTradeWithStop = trade;
				orderEnsurer = new OrderEnsurer(BuildOrder(currentTradeWithStop));
				Task.Run(WaitForCurrentToCompleteAndSendStop);
			}
			else
			{
				if(killingAttempt == null)
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

		private void WaitForCurrentToCompleteAndSendStop()
		{
			WaitForArrival();
			while (!Task.Run(() => { while (!orderEnsurer.IsComplete) Task.Delay(5).Wait(); }).Wait(30))
			{
				orderEnsurer.UpdateOrderFromQuikByTransID();
			}
			if (orderEnsurer.State == EnsuranceState.Executed)
			{
				stopStorage.OnNewTradeWithStop(
					currentTradeWithStop
					);
				NewTradeWithStop?.Invoke(currentTradeWithStop);
			}
			goodToGo = true;
		}

		Order BuildOrder(Trading.Trade trade)
		{
			return QuikHelpFunctions.BuildOrder(trade, tradeParams, 1);
		}

		StopOrder BuildStopOrder(Trading.TradeWithStop trade)
		{
			return QuikHelpFunctions.BuildStopOrder(trade, tradeParams, 1);
		}
	}
}
