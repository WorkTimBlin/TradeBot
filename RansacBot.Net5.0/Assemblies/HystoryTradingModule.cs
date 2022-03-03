using RansacBot.Trading;
using RansacBot.Trading.Hystory;
using RansacBot.Trading.Hystory.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	class HystoryTradingModule : AbstractTradingModule<HystoryOrder, HystoryOrder>
	{
		public HystoryQuikSimulator QuikSimulator { get; } = new();
		public HystoryTradingModule() : base() { }
		public HystoryTradingModule(
			ITradeWithStopProvider tradeWithStopProvider,
			IClosingProvider closingProvider) : base(
				tradeWithStopProvider,
				closingProvider)
		{ }

		protected override AbstractOneAtATimeCheckpoint<HystoryOrder> GetCheckpoint()
		{
			return new HystoryOneAtATimeCheckpoint(QuikSimulator);
		}

		protected override AbstractKilledStopsMarketCompensator<HystoryOrder> GetCompensator()
		{
			return new HystoryKilledStopsMarketCompensator(QuikSimulator);
		}

		protected override AbstractClassicStopsOperator<HystoryOrder, HystoryOrder> GetStopsOperator()
		{
			return new HystoryStopsOperator(QuikSimulator);
		}
	}
}
