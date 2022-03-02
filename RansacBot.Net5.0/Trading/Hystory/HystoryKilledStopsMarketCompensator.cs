using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class HystoryKilledStopsMarketCompensator : AbstractKilledStopsMarketCompensator<HystoryOrder>
	{
		protected override AbstractOrderEnsurerWithPrice<HystoryOrder> GetMarketEnsurer(TradeWithStop trade)
		{
			return
				new HystoryOrderEnsurer(
					new(
						new(
							trade.direction == TradeDirection.buy ? Double.MaxValue : Double.MinValue,
							trade.direction)));
		}
	}
}
