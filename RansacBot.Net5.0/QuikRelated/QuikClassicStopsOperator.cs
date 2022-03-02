using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.QuikRelated
{
	class QuikClassicStopsOperator : AbstractClassicStopsOperator<StopOrder, Order>
	{
		TradeParams tradeParams;

		public QuikClassicStopsOperator(TradeParams tradeParams) :
			base((ensurer1, ensurer2) => ensurer1.Order.Price > ensurer2.Order.Price ? 1 : -1,
				(ensurer1, ensurer2) => ensurer1.Order.Price < ensurer2.Order.Price ? 1 : -1)
		{
			this.tradeParams = tradeParams;
		}


		protected override AbstractStopOrderEnsurer<StopOrder, Order> BuildStopOrderEnsurer(TradeWithStop trade)
		{
			return new QuikStopOrderEnsurer(QuikHelpFunctions.BuildStopOrder(trade, tradeParams, 1));
		}
		protected override AbstractOrderEnsurerWithPrice<Order> GetOrderEnsurer(Order order)
		{
			return new QuikOrderEnsurer(order);
		}

		public override string GetSerialized(AbstractStopOrderEnsurer<StopOrder, Order> stopOrder)
		{
			return stopOrder.Order.TransId.ToString() + " " + stopOrder.Order.ConditionPrice.ToString();
		}

		protected override bool IsLong(AbstractStopOrderEnsurer<StopOrder, Order> stopEnsurer)
		{
			return stopEnsurer.Order.IsLong();
		}

		public override string GetSerialized(AbstractOrderEnsurerWithPrice<Order> ensurer)
		{
			return ensurer.Order.TransID.ToString() + " " + ensurer.Order.Price.ToString();
		}
	}
}
