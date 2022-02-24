using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	/// <summary>
	/// let TradeWithStop through when it's completed, kills current if got new while current isn't executed
	/// passed TradeWithStop transforms corresponding to execution price
	/// </summary>
	/// <typeparam name="TOrder"></typeparam>
	abstract class AbstractOneAtATimeCheckpoint<TOrder> : ITradeWithStopFilter
	{
		public event Action<TradeWithStop> NewTradeWithStop;

		AbstractOrderEnsurerWithPrice<TOrder> orderEnsurer;

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

		private void OnOrderEnsuranceStatusChanged(object orderEnsurer) =>
			OnOrderEnsuranceStatusChanged(
				orderEnsurer as AbstractOrderEnsurerWithPrice<TOrder> ??
				throw new TypeAccessException("cant cast orderEnsurer To right Type"));
		private void OnOrderEnsuranceStatusChanged(AbstractOrderEnsurerWithPrice<TOrder> orderEnsurer)
		{
			if (orderEnsurer.IsComplete)
			{
				if(orderEnsurer.State == EnsuranceState.Executed)
				{
					if (orderEnsurer.CompletionAttribute == 0) return;
					NewTradeWithStop?.Invoke(GetCurrentTradeWithStopWithRepalcedPrice(orderEnsurer.CompletionAttribute));
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
		protected abstract AbstractOrderEnsurerWithPrice<TOrder> GetNewOrderEnsurer(Trading.Trade trade);
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
