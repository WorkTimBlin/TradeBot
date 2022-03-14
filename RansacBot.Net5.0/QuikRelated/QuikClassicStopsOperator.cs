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
			base(
				LongComparison,
				ShortComparison
				)
		{
			this.tradeParams = tradeParams;
		}

		protected override StopOrder BuildStopOrder(TradeWithStop tradeWithStop)
		{
			return QuikHelpFunctions.BuildStopOrder(tradeWithStop, tradeParams, 1);
		}

		protected override bool IsLong(StopOrder stopOrder)
		{
			return stopOrder.IsLong();
		}

		protected override AbstractSortedOrdersWaitroom<StopOrder, Order> 
			GetNewWaitroom(Comparison<AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order>> comparison)
		{
			return new QuikStopWaitroom(comparison);
		}

		protected override AbstractSentOrdersWaitroom<Order, double> GetNewExecutedWaitroom()
		{
			return new QuikOrderWaitroom();
		}
		private static int LongComparison(
			AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order> ensurer1,
			AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order> ensurer2
			)
		{
			if (ensurer1.Order.Price > ensurer2.Order.Price) return 1;
			if (ensurer1.Order.Price < ensurer2.Order.Price) return -1;
			if (ensurer1.Order.TransId > ensurer2.Order.TransId) return 1;
			else return -1;
		}
		private static int ShortComparison(
			AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order> ensurer1,
			AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order> ensurer2
			)
		{
			if (ensurer1.Order.Price < ensurer2.Order.Price) return 1;
			if (ensurer1.Order.Price > ensurer2.Order.Price) return -1;
			if (ensurer1.Order.TransId > ensurer2.Order.TransId) return 1;
			else return -1;
		}

		class QuikOrderWaitroom : AbstractSentOrdersWaitroom<Order, double>
		{
			public override string GetSerialized(AbstractOrderEnsurerWithCompletionAttribute<Order, double> ensurer)
			{
				return ensurer.Order.TransID.ToString() + " " + ensurer.Order.Price.ToString();
			}

			protected override AbstractOrderEnsurerWithCompletionAttribute<Order, double> GetNewEnsurer(Order order)
			{
				return new QuikOrderEnsurer(order);
			}
		}
		class QuikStopWaitroom : AbstractSortedOrdersWaitroom<StopOrder, Order>
		{
			public QuikStopWaitroom(Comparison<AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order>> compartion) : base(compartion)
			{ }
			public override string GetSerialized(AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order> ensurer)
			{
				return ensurer.Order.TransId.ToString() + " " + ensurer.Order.Price.ToString();
			}

			protected override AbstractOrderEnsurerWithCompletionAttribute<StopOrder, Order> GetEnsurer(StopOrder order)
			{
				return new QuikStopOrderEnsurer(order);
			}
		}
	}
}
