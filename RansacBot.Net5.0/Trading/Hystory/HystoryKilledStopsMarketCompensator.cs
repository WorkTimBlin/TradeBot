using RansacBot.Trading.Hystory.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class HystoryKilledStopsMarketCompensator : AbstractKilledStopsMarketCompensator<HystoryOrder>
	{
		HystoryQuikSimulator quikSimulator;
		public HystoryKilledStopsMarketCompensator(HystoryQuikSimulator quikSimulator)
		{
			this.quikSimulator = quikSimulator;
		}
		protected override AbstractOrderEnsurerWithPrice<HystoryOrder> GetMarketEnsurer(TradeWithStop trade)
		{
			return
				new HystoryOrderEnsurer(
					new(
						new(
							trade.direction == TradeDirection.buy ? Double.MaxValue : Double.MinValue,
							trade.direction)), 
					quikSimulator);
		}
	}
}
