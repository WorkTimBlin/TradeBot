using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class HystoryStopsOperator : AbstractClassicStopsOperator<HystoryOrder, HystoryOrder>
	{
		public HystoryStopsOperator() : 
			base(
				(ensurer1, ensurer2) => ensurer1.Order.price > ensurer2.Order.price ? 1 : -1,
				(ensurer1, ensurer2) => ensurer1.Order.price < ensurer2.Order.price ? 1 : -1)
		{ }

		public override string GetSerialized(AbstractStopOrderEnsurer<HystoryOrder, HystoryOrder> stopOrder)
		{
			return stopOrder.Order.transID.ToString() + " " + stopOrder.Order.price.ToString();
		}

		public override string GetSerialized(AbstractOrderEnsurerWithPrice<HystoryOrder> ensurer)
		{
			return ensurer.Order.transID.ToString() + " " + ensurer.Order.transID.ToString();
		}

		protected override AbstractStopOrderEnsurer<HystoryOrder, HystoryOrder> 
			BuildStopOrderEnsurer(TradeWithStop trade)
		{
			return new HystoryStopOrderEnsurer(new(trade.stop));
		}

		protected override AbstractOrderEnsurerWithPrice<HystoryOrder> GetOrderEnsurer(HystoryOrder order)
		{
			return new HystoryOrderEnsurer(order);
		}

		protected override bool IsLong(AbstractStopOrderEnsurer<HystoryOrder, HystoryOrder> stopEnsurer)
		{
			return stopEnsurer.Order.direction == TradeDirection.sell;
		}
	}
}
