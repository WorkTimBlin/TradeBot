using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;
using RansacBot.Trading;

namespace RansacBot.QuikRelated
{
	class QuikKilledStopsMarketCompensator : AbstractKilledStopsMarketCompensator<Order>
	{
		TradeParams tradeParams;

		public QuikKilledStopsMarketCompensator(TradeParams tradeParams)
		{
			this.tradeParams = tradeParams;
		}

		protected override AbstractOrderEnsurerWithPrice<Order> GetMarketEnsurer(TradeWithStop trade)
		{
			return new QuikOrderEnsurer(QuikHelpFunctions.BuildMarketOrder(trade.GetOperation(), tradeParams, 1));
		}
	}
}
