using QuikSharp.DataStructures;
using QuikSharp;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;

namespace RansacBot.QuikRelated
{
	abstract class AbstractOrderEnsurer<TOrder>
	{
		public TOrder Order { get; private set; }
		public EnsuranceState State { get; private set; } = EnsuranceState.NotSentYet;
		public bool IsComplete { get { return State == EnsuranceState.Executed || State == EnsuranceState.Killed; } }

		protected IOrderFunctions<TOrder> functions;

		public AbstractOrderEnsurer(TOrder order, IOrderFunctions<TOrder> orderFunctions):this(order)
		{
			functions = orderFunctions;
		}
		public AbstractOrderEnsurer(TOrder order)
		{
			this.Order = order;
		}

		protected void SubscribeSelfAndSendOrder()
		{
			if(State > EnsuranceState.Sent)
			{
				throw new StateException("Order is already in quik");
			}
			SubscribeToOnOrderEvent();

			SetTransID(functions.CreateOrder(Order).Result);
			ChangeStateTo(EnsuranceState.Sent);
		}
		public void Kill()
		{
			UpdateOrderFromQuikByTransID();
			if (State != EnsuranceState.Active)
			{
				Console.WriteLine("error during killing");
				throw new StateException("Can't kill unactive order");
			}
			functions.KillOrder(Order);
			ChangeStateTo(EnsuranceState.Killing);
		}
		public void UpdateOrderFromQuikByTransID()
		{
			OnOrderChanged(functions.GetOrder_by_transID(Order).Result);
		}
		void ChangeStateTo(EnsuranceState state)
		{
			this.State = state;
		}

		protected void OnOrderChanged(TOrder order)
		{
			if (!IsTransIDMatching(order)) return;
			Order = order;
			if (GetState(Order) == QuikSharp.DataStructures.State.Active)
			{
				ChangeStateTo(EnsuranceState.Active);
			}
			if (GetState(Order) == QuikSharp.DataStructures.State.Completed)
			{
				ChangeStateTo(EnsuranceState.Executed);
				UnsubscribeFromOnOrderEvent();
			}
			if (GetState(Order) == QuikSharp.DataStructures.State.Canceled)
			{
				ChangeStateTo(EnsuranceState.Killed);
				UnsubscribeFromOnOrderEvent();
			}
		}

		protected abstract void SubscribeToOnOrderEvent();
		protected abstract void UnsubscribeFromOnOrderEvent();
		protected abstract State GetState(TOrder order);
		protected abstract void SetTransID(long transID);
		protected abstract bool IsTransIDMatching(TOrder order);
	}

	class StateException : Exception
	{
		public StateException(string message) : base(message) { }
	}
	enum EnsuranceState
	{
		NotSentYet,
		Sent,
		Active,
		Killing,
		Executed,
		Killed
	}

	public interface IOrderFunctions<TOrder>
	{
		Task<long> CreateOrder(TOrder order);
		Task<long> KillOrder(TOrder order);
		Task<TOrder?> GetOrder_by_transID(TOrder order);
	}
}
