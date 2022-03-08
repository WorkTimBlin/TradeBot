using RansacBot.Trading.Hystory.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class HystoryOneAtATimeCheckpoint : AbstractOneAtATimeCheckpoint<HystoryOrder>
	{
		HystoryQuikSimulator quikSimulator;

		public HystoryOneAtATimeCheckpoint(HystoryQuikSimulator quikSimulator)
		{
			this.quikSimulator = quikSimulator;
		}

		protected override void PerformKilling()
		{
			KillSyncronously();
		}
		protected override AbstractOrderEnsurerWithPrice<HystoryOrder> GetNewOrderEnsurer(Trade trade)
		{
			return new HystoryOrderEnsurer(new(trade), quikSimulator);
		}
	}
}
