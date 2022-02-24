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
				(ensurer1, ensurer2) => ensurer1.Order.Price > ensurer2.Order.Price ? 1 : -1)
		{
			this.tradeParams = tradeParams;
		}


		protected override SortedDictionary<AbstractStopOrderEnsurer<StopOrder, Order>, TradeWithStop> 
			GetDict(AbstractStopOrderEnsurer<StopOrder, Order> ensurer)
		{
			return ensurer.Order.IsLong() ? longs : shorts;
		}
		public override List<string> GetLongs()
		{
			return new(
				longs.Keys.Select(
					(stopOrder) =>
					{ return stopOrder.Order.TransId.ToString() + " " + stopOrder.Order.ConditionPrice.ToString(); }));
		}

		public override List<string> GetShorts()
		{
			return new(
				shorts.Keys.Select(
					(stopOrder) =>
					{ return stopOrder.Order.TransId.ToString() + " " + stopOrder.Order.ConditionPrice.ToString(); }));
		}

		protected override AbstractStopOrderEnsurer<StopOrder, Order> BuildStopOrderEnsurer(TradeWithStop trade)
		{
			return new QuikStopOrderEnsurer(QuikHelpFunctions.BuildStopOrder(trade, tradeParams, 1));
		}
		protected override AbstractOrderEnsurerWithPrice<Order> GetOrderEnsurer(Order order)
		{
			return new QuikOrderEnsurer(order);
		}
	}
}
