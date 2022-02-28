using QuikSharp.DataStructures;
using QuikSharp;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;

namespace RansacBot.Trading
{
	public abstract class AbstractOrderEnsurer<TOrder>
	{
		public event Action<object> OrderEnsuranceStatusChanged;
		public TOrder Order { get; protected set; }
		public EnsuranceState State { get; private set; } = EnsuranceState.NotSentYet;
		public bool IsComplete { get { return State == EnsuranceState.Executed || State == EnsuranceState.Killed; } }

		protected IOrderFunctions<TOrder> functions;

		public AbstractOrderEnsurer(TOrder order, IOrderFunctions<TOrder> orderFunctions):this(order)
		{
			functions = orderFunctions;
		}
		private AbstractOrderEnsurer(TOrder order)
		{
			this.Order = order;
		}

		public virtual void Subscribe()
		{
			UnsubscribeFromOnOrderEvent();
			SubscribeToOnOrderEvent();
		}
		public void SubscribeSelfAndSendOrder()
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
			TOrder? order = functions.GetOrder_by_transID(Order).Result;
			if(order != null)
				OnOrderChanged(order);
		}

		protected void OnOrderChanged(TOrder order)
		{
			if (!IsTransIDMatching(order)) return;
			Order = order;
			if (GetState(Order) == QuikSharp.DataStructures.State.Active)
			{
				OnActive();
			}
			if (GetState(Order) == QuikSharp.DataStructures.State.Completed)
			{
				UnsubscribeFromOnOrderEvent();
				OnCompleted();
			}
			if (GetState(Order) == QuikSharp.DataStructures.State.Canceled)
			{
				UnsubscribeFromOnOrderEvent();
				OnKilled();
			}
		}
		protected void ChangeStateTo(EnsuranceState state)
		{
			this.State = state;
			OrderEnsuranceStatusChanged?.Invoke(this);
		}


		protected virtual void OnActive()
		{
			ChangeStateTo(EnsuranceState.Active);
		}
		protected virtual void OnCompleted()
		{
			ChangeStateTo(EnsuranceState.Executed);
		}
		protected virtual void OnKilled()
		{
			ChangeStateTo(EnsuranceState.Killed);
		}

		protected abstract void SubscribeToOnOrderEvent();
		protected abstract void UnsubscribeFromOnOrderEvent();
		protected abstract State GetState(TOrder order);
		protected abstract void SetTransID(long transID);
		protected abstract bool IsTransIDMatching(TOrder order);
		public bool IsSame(TOrder order) => IsTransIDMatching(order);
	}

	class StateException : Exception
	{
		public StateException(string message) : base(message) { }
	}
	public enum EnsuranceState
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
