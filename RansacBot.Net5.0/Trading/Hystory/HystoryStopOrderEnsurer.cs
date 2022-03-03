using QuikSharp.DataStructures;
using RansacBot.Trading.Hystory.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class HystoryStopOrderEnsurer : AbstractStopOrderEnsurer<HystoryOrder, HystoryOrder>
	{
		readonly IHystoryOrderEvents orderEvents;

		public HystoryStopOrderEnsurer(HystoryOrder order, Infrastructure.HystoryQuikSimulator quikSimulator) :
			base(order, quikSimulator.Stops)
		{
			orderEvents = quikSimulator.Stops;
		}

		protected override HystoryOrder GetCompletionAttribute()
		{
			return Order;
		}
		protected override State GetState(HystoryOrder order)
		{
			return Order.state;
		}
		protected override bool IsTransIDMatching(HystoryOrder order)
		{
			return order.transID == Order.transID;
		}
		protected override void SetTransID(long transID)
		{
			Order.transID = transID;
		}
		protected override void SubscribeToOnOrderEvent()
		{
			orderEvents.OrderChanged += OnOrderChanged;
		}
		protected override void UnsubscribeFromOnOrderEvent()
		{
			orderEvents.OrderChanged -= OnOrderChanged;
		}
	}
}
