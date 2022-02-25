using QuikSharp.DataStructures;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory
{
	class OrderOnHystoryEnsurer : AbstractOrderEnsurerWithPrice<HystoryOrder>
	{
		IOrderEvents orderEvents;
		public OrderOnHystoryEnsurer(HystoryOrder trade) : 
			base(trade, Infrastructure.HystoryQuikSimulator.Instance.Orders)
		{
			throw new NotImplementedException();
		}

		protected override double GetCompletionAttribute()
		{
			return Order.completionPrice;
		}

		protected override State GetState(HystoryOrder order)
		{
			return order.state;
		}
		protected override void SetTransID(long transID)
		{
			Order.transID = transID;
		}
		protected override bool IsTransIDMatching(HystoryOrder order) => order.transID == Order.transID;

		//don't need this funcs because order is updating itself
		
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
