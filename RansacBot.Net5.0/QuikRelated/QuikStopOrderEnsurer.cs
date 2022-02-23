using QuikSharp;
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
	/// <summary>
	/// can call orderEnsuranceStatusChanged twice - once when got an execution price
	/// </summary>
	class QuikStopOrderEnsurer : AbstractStopOrderEnsurer<StopOrder, Order>
	{
		IQuikEvents events;

		public QuikStopOrderEnsurer(StopOrder stopOrder):base(stopOrder, QuikStopOrderFunctions.Instance)
		{
			this.events = QuikContainer.Quik.Events;
			//SubscribeSelfAndSendOrder();
		}

		protected override Order GetCompletionAttribute()
		{
			return QuikHelpFunctions.GetOrderByTransID(Order.ClassCode, Order.SecCode, Order.TransId) ?? 
				throw new Exception("no such order in quik");
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
			public async Task<StopOrder?> GetOrderByTransID(string classCode, string securityCode, long transID)
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
