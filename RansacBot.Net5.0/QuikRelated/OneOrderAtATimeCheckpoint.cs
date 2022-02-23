using QuikSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacBot.Trading;
using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;

namespace RansacBot.QuikRelated
{
	class OneOrderAtATimeCheckpoint : AbstractOneAtATimeCheckpoint<Order>
	{
		readonly TradeParams tradeParams;

		public OneOrderAtATimeCheckpoint (TradeParams tradeParams) : base()
		{
			this.tradeParams = tradeParams;
		}

		protected override AbstractOrderEnsurerWithPrice<Order> GetNewOrderEnsurer(Trading.Trade trade) => 
			new QuikOrderEnsurer(BuildOrder(trade));

		Order BuildOrder(Trading.Trade trade)
		{
			return QuikHelpFunctions.BuildOrder(trade, tradeParams, 1);
		}
	}
}
