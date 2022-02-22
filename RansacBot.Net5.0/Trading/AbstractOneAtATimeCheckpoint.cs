using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	/// <summary>
	/// let TradeWithStop through exactly and only when order ensurer gives a callback having non-zero execution price
	/// </summary>
	/// <typeparam name="TOrder"></typeparam>
	abstract class AbstractOneAtATimeCheckpoint<TOrder> : ITradeWithStopFilter
	{
		public event Action<TradeWithStop> NewTradeWithStop;

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
				orderEnsurer.OrderEnsuranceStatusChanged += OnOrderEnsuranceStatusChanged;
				orderEnsurer.SubscribeSelfAndSendOrder();
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

		private void OnOrderEnsuranceStatusChanged(AbstractOrderEnsurer<TOrder> orderEnsurer)
		{
			if (orderEnsurer.IsComplete)
			{
				if(orderEnsurer.State == EnsuranceState.Executed)
				{
					if (orderEnsurer.ExecutionPrice == 0) return;
					NewTradeWithStop?.Invoke(GetCurrentTradeWithStopWithRepalcedPrice(orderEnsurer.ExecutionPrice));
				}
				orderEnsurer.OrderEnsuranceStatusChanged -= OnOrderEnsuranceStatusChanged;
				goodToGo = true;
			}
		}

		protected TradeWithStop GetCurrentTradeWithStopWithRepalcedPrice(double price)
		{
			return new(new(price, currentTradeWithStop.direction), currentTradeWithStop.stop.price);
		}

		//protected abstract TOrderEnsurer GetNewOrderEnsurer<TOrderEnsurer>(Trading.Trade trade)
		//where TOrderEnsurer : AbstractOrderEnsurer<TOrder>;
		protected abstract AbstractOrderEnsurer<TOrder> GetNewOrderEnsurer(Trading.Trade trade);
	}

	public class TimerUpdater : Task
	{
		public TimerUpdater(
			Func<bool> awaitingCondition,
			Func<bool> cancellationCondition,
			Action Update,
			int periodInMillisecons):
			base(() =>
			{
				Task awaitingTimer = Task.Run(() =>
				{
					while (awaitingCondition()) ;
				});
				while (!cancellationCondition() && awaitingTimer.Wait(periodInMillisecons) && !cancellationCondition())
				{
					Update();
				}
			})
		{ }
	}
}
