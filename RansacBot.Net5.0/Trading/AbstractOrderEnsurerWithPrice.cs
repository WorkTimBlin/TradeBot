using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	abstract class AbstractOrderEnsurerWithCompletionAttribute<TOrder, TCompletionAttribute> : 
		AbstractOrderEnsurer<TOrder>
	{
		public TCompletionAttribute CompletionAttribute { get; protected set; }

		public AbstractOrderEnsurerWithCompletionAttribute(TOrder order, IOrderFunctions<TOrder> orderFunctions) : 
			base(order, orderFunctions) { }

		protected override void OnCompleted()
		{
			CompletionAttribute = GetCompletionAttribute();
			base.OnCompleted();
		}

		protected abstract TCompletionAttribute GetCompletionAttribute();
	}

	abstract class AbstractOrderEnsurerWithPrice<TOrder> : AbstractOrderEnsurerWithCompletionAttribute<TOrder, double>
	{
		public AbstractOrderEnsurerWithPrice(TOrder order, IOrderFunctions<TOrder> orderFunctions) : 
			base(order, orderFunctions) { }
	}

	abstract class AbstractStopOrderEnsurer<TStopOrder, TOrder> : 
		AbstractOrderEnsurerWithCompletionAttribute<
			TStopOrder, 
			TOrder>
	{
		public AbstractStopOrderEnsurer(TStopOrder order, IOrderFunctions<TStopOrder> orderFunctions) : 
			base(order, orderFunctions)
		{ }
	}
}
