using QuikSharp;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.QuikRelated
{
	class StopOrderEnsurer : AbstractOrderEnsurer<StopOrder>
	{
		IQuikEvents events;

		public StopOrderEnsurer(StopOrder stopOrder):base(stopOrder, QuikStopOrderFunctions.Instance)
		{
			this.events = QuikContainer.Quik.Events;
			SubscribeSelfAndSendOrder();
		}

		protected override State GetState(StopOrder order)
		{
			return order.State;
		}
		protected override bool IsTransIDMatching(StopOrder order)
		{
			return order != null && order.TransId == Order.TransId;
		}
		protected override void SetTransID(long transID)
		{
			Order.TransId = transID;
		}
		protected override void SubscribeToOnOrderEvent()
		{
			events.OnStopOrder += OnOrderChanged;
		}
		protected override void UnsubscribeFromOnOrderEvent()
		{
			events.OnStopOrder -= OnOrderChanged;
		}

		public class QuikStopOrderFunctions : IOrderFunctions<StopOrder>
		{
			public static QuikStopOrderFunctions Instance { get { return _instance; } }
			static QuikStopOrderFunctions _instance = new();

			static StopOrderFunctions func = QuikContainer.Quik.StopOrders;

			private QuikStopOrderFunctions() { }
			public async Task<long> CreateOrder(StopOrder order)
			{
				return await func.CreateStopOrder(order);
			}
			public async Task<StopOrder?> GetOrder_by_transID(StopOrder order)
			{
				return await GetOrderByTransID(order.ClassCode, order.SecCode, order.TransId);
			}
			private async Task<StopOrder?> GetOrderByTransID(string classCode, string securityCode, long transID)
			{
				return (await func.GetStopOrders(classCode, securityCode)).
						Find((stopOrder) => stopOrder.TransId == transID)
					?? null;
			}
			async public Task<long> KillOrder(StopOrder order)
			{
				return await func.KillStopOrder(order);
			}
		}
	}
}
