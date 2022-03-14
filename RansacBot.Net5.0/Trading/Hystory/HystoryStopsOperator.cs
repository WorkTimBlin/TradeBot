using RansacBot.Trading.Hystory.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class HystoryStopsOperator : AbstractClassicStopsOperator<HystoryOrder, HystoryOrder>
	{
		HystoryQuikSimulator quikSimulator;
		public HystoryStopsOperator(HystoryQuikSimulator quikSimulator) :
			base(LongComparison, ShortComparison)
		{
			this.quikSimulator = quikSimulator;
		}

		protected override HystoryOrder BuildStopOrder(TradeWithStop tradeWithStop)
		{
			return new HystoryOrder(tradeWithStop.stop);
		}

		protected override AbstractSentOrdersWaitroom<HystoryOrder, double> GetNewExecutedWaitroom()
		{
			return new ExecutedWaitroom(GetHystoryQuikSimulator);
		}

		protected override AbstractSortedOrdersWaitroom<HystoryOrder, HystoryOrder> 
			GetNewWaitroom(Comparison<AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder>> comparison)
		{
			return new StopWaitroom(comparison, GetHystoryQuikSimulator);
		}

		protected override bool IsLong(HystoryOrder stopOrder)
		{
			return stopOrder.direction == TradeDirection.sell;
		}

		private HystoryQuikSimulator GetHystoryQuikSimulator() => quikSimulator;
		private static int LongComparison(
			AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder> ensurer1,
			AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder> ensurer2
			)
		{
			if (ensurer1.Order.price > ensurer2.Order.price) return 1;
			if (ensurer1.Order.price < ensurer2.Order.price) return -1;
			if (ensurer1.Order.transID > ensurer2.Order.transID) return 1;
			if (ensurer1.Order.transID < ensurer2.Order.transID) return -1;
			return 0;
		}
		private static int ShortComparison(
			AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder> ensurer1,
			AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder> ensurer2
			)
		{
			if (ensurer1.Order.price < ensurer2.Order.price) return 1;
			if (ensurer1.Order.price > ensurer2.Order.price) return -1;
			if (ensurer1.Order.transID > ensurer2.Order.transID) return 1;
			if (ensurer1.Order.transID < ensurer2.Order.transID) return -1;
			return 0;
		}

		class ExecutedWaitroom : AbstractSentOrdersWaitroom<HystoryOrder, double>
		{
			Func<HystoryQuikSimulator> quikSimulatorGetter;
			public ExecutedWaitroom(Func<HystoryQuikSimulator> quikSimulatorGetter)
			{
				this.quikSimulatorGetter = quikSimulatorGetter;
			}
			public override string GetSerialized(AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, double> ensurer)
			{
				return ensurer.Order.transID.ToString() + " " + ensurer.Order.transID.ToString();
			}

			protected override AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, double> GetNewEnsurer(HystoryOrder order)
			{
				return new HystoryOrderEnsurer(order, quikSimulatorGetter());
			}
		}
		class StopWaitroom : AbstractSortedOrdersWaitroom<HystoryOrder, HystoryOrder>
		{
			Func<HystoryQuikSimulator> quikSimulatorGetter;
			public StopWaitroom(
				Comparison<AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder>> comparison, 
				Func<HystoryQuikSimulator> quikSimulatorGetter) : base(comparison)
			{
				this.quikSimulatorGetter = quikSimulatorGetter;
			}

			public override string GetSerialized(AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder> ensurer)
			{
				return ensurer.Order.transID.ToString() + " " + ensurer.Order.transID.ToString();
			}

			protected override AbstractOrderEnsurerWithCompletionAttribute<HystoryOrder, HystoryOrder> GetEnsurer(HystoryOrder order)
			{
				return new HystoryStopOrderEnsurer(order, quikSimulatorGetter());
			}
		}
	}
}
