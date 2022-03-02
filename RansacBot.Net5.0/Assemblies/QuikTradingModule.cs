using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using RansacBot.QuikRelated;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	class QuikTradingModule : AbstractTradingModule<Order, StopOrder>
	{
		TradeParams tradeParams;
		public QuikTradingModule(TradeParams tradeParams, 
			ITradeWithStopProvider tradeWithStopProvider, 
			IClosingProvider closingProvider) :
			base(tradeWithStopProvider, closingProvider)
		{
			this.tradeParams = tradeParams;
		}

		public QuikTradingModule(TradeParams tradeParams) : base()
		{
			this.tradeParams = tradeParams;
		}

		protected override AbstractOneAtATimeCheckpoint<Order> GetCheckpoint()
		{
			return new QuikOneOrderAtATimeCheckpoint(tradeParams);
		}
		protected override AbstractClassicStopsOperator<StopOrder, Order> GetStopsOperator()
		{
			return new QuikClassicStopsOperator(tradeParams);
		}
		protected override AbstractKilledStopsMarketCompensator<Order> GetCompensator()
		{
			return new QuikKilledStopsMarketCompensator(tradeParams);
		}
	}
}
