﻿using QuikSharp;
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
	/// warning: can call OrderEnsuranceStatusChanged twice completed - once when got an execution price
	/// </summary>
	class OrderEnsurer : AbstractOrderEnsurer<Order>
	{
		IQuikEvents events;
		
		public OrderEnsurer(Order order) : base(order, QuikOrderFunctions.Instance)
		{
			events = QuikContainer.Quik.Events;
			SubscribeToOnTradeEvent();
			//SubscribeSelfAndSendOrder();
		}

		public void UpdateCompletionPrice()
		{
			OnNewTrade(
				QuikHelpFunctions.GetTradeByTransID(Order.TransID) ?? 
				throw new Exception("no trade with such transID found"));
		}

		private void OnNewTrade(QuikSharp.DataStructures.Transaction.Trade trade)
		{
			if (trade.TransID != Order.TransID) return;
			UnsubscribeFromOnTradeEvent();
			ExecutionPrice = trade.Price;
			UpdateOrderFromQuikByTransID();
		}
		private void SubscribeToOnTradeEvent()
		{
			events.OnTrade += OnNewTrade;
		}
		private void UnsubscribeFromOnTradeEvent()
		{
			events.OnTrade -= OnNewTrade;
		}

		protected override State GetState(Order order)
		{
			return Order.State;
		}
		protected override bool IsTransIDMatching(Order order)
		{
			return order != null && order.TransID == Order.TransID;
		}
		protected override void SetTransID(long transID)
		{
			Order.TransID = transID;
		}
		protected override void SubscribeToOnOrderEvent()
		{
			events.OnOrder += OnOrderChanged;
		}
		protected override void UnsubscribeFromOnOrderEvent()
		{
			events.OnOrder -= OnOrderChanged;
		}

		public class QuikOrderFunctions : IOrderFunctions<Order>
		{
			static QuikOrderFunctions _instance = new();
			static public QuikOrderFunctions Instance { get { return _instance; } }

			private QuikOrderFunctions() { }

			static OrderFunctions func = QuikContainer.Quik.Orders;
			async public Task<long> CreateOrder(Order order)
			{
				return await func.CreateOrder(order);
			}

			async public Task<Order?> GetOrder_by_transID(Order order)
			{
				return await func.GetOrder_by_transID(order.ClassCode, order.SecCode, order.TransID);
			}

			async public Task<Order> GetOrderByTransID(string classCode, string securityCode, long transID)
			{
				List<Order> orders = await func.GetOrders(classCode, securityCode);
				return func.GetOrders(classCode, securityCode).Result.
					Find((order) => order.TransID == transID) ?? throw new Exception("could't find such order");
				//return await func.GetOrder_by_transID(classCode, securityCode, transID);
			}

			async public Task<long> KillOrder(Order order)
			{
				return await func.KillOrder(order);
			}
		}
	}
}
