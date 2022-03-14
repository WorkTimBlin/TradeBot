using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	abstract class AbstractSentOrdersWaitroom<TOrder, TCompletionAttribute>
	{
		public event Action<TradeWithStop, TCompletionAttribute> OrderExecuted;
		public event Action<TradeWithStop> OrderKilled;
		private Dictionary<AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute>, TradeWithStop> ensurers = new();
		public void OnNewSentOrder(TradeWithStop tradeWithStop, TOrder order)
		{
			AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute> ensurer = GetNewEnsurer(order);
			ensurer.OrderEnsuranceStatusChanged += OnOrderEnsuranceStatusChanged;
			ensurer.Subscribe();
			ensurers.Add(ensurer, tradeWithStop);

			if (ensurer.IsComplete) OnOrderEnsuranceStatusChanged(ensurer);
			else ensurer.UpdateOrderFromQuikByTransID();
		}
		private void OnOrderEnsuranceStatusChanged(object aEnsurer)
		{
			AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute> ensurer = 
				aEnsurer as AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute> ?? 
				throw new Exception("can't cast ensurer to the right type");
			if (ensurer.IsComplete)
			{
				ensurer.OrderEnsuranceStatusChanged -= OnOrderEnsuranceStatusChanged;
				if (ensurer.State == EnsuranceState.Killed)
				{
					OrderKilled?.Invoke(ensurers[ensurer]);
				}
				if (ensurer.State == EnsuranceState.Executed)
				{
					OrderExecuted?.Invoke(ensurers[ensurer], ensurer.CompletionAttribute);
				}
				ensurers.Remove(ensurer);
			}
		}
		public void KillLastPercent(double percent)
		{
			List<AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute>> ordersList = ensurers.Keys.ToList();
			for (int i = ordersList.Count - 1; i >= (int)(ordersList.Count * (100 - percent) / 100); i--)
			{
				AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute> stop = ordersList[i];
				stop.UpdateOrderFromQuikByTransID();
				if (stop.State == EnsuranceState.Active)
				{
					stop.Kill();
				}
			}
		}
		public IEnumerable<string> GetAsStrings()
		{
			return ensurers.Keys.Select(GetSerialized);
		}
		public abstract string GetSerialized(AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute> ensurer);
		protected abstract AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute> GetNewEnsurer(TOrder order);
	}
}
