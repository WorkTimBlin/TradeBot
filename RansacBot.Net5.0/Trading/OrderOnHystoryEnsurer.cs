using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	class OrderOnHystoryEnsurer : AbstractOrderEnsurer<TradeOrder>
	{
		public OrderOnHystoryEnsurer(TradeOrder trade):base(trade, HystoryTradeFunc.Instance) { }

		protected override State GetState(TradeOrder order)
		{
			throw new NotImplementedException();
		}

		protected override bool IsTransIDMatching(TradeOrder order)
		{
			throw new NotImplementedException();
		}

		protected override void SetTransID(long transID)
		{
			throw new NotImplementedException();
		}

		protected override void SubscribeToOnOrderEvent()
		{
			throw new NotImplementedException();
		}

		protected override void UnsubscribeFromOnOrderEvent()
		{
			throw new NotImplementedException();
		}
	}

	class HystoryTradeFunc : IOrderFunctions<TradeOrder>
	{
		public static HystoryTradeFunc Instance { get; } = new();
		private HystoryTradeFunc() { }

		public async Task<long> CreateOrder(TradeOrder order)
		{
			return 0;
		}

		public async Task<TradeOrder?> GetOrder_by_transID(TradeOrder order)
		{
			return order;
		}

		public async Task<long> KillOrder(TradeOrder order)
		{
			return 0;
		}

		List<TradeOrder> trades;
	}
}
