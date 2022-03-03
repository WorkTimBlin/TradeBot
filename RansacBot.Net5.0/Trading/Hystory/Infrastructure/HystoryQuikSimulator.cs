using QuikSharp;
using RansacBot.Trading;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Hystory.Infrastructure
{
	class HystoryQuikSimulator : ITickFilter
	{
		public static HystoryQuikSimulator Instance { get; private set; } = new();

		public event Action<Tick> NewTick;

		public OrdersContainer Orders { get; }
		public StopsContainer Stops { get; }
		public HystoryQuikSimulator()
		{
			Orders = new(GetNewTransID);
			Stops = new(GetNewTransID);
			Stops.OrderChanged += CreateOrderFromStopIfExecuted;
		}

		public void OnNewTick(Tick tick)
		{
			Orders.OnNewTick(tick);
			Stops.OnNewTick(tick);
			NewTick?.Invoke(tick);
		}
		
		private void CreateOrderFromStopIfExecuted(HystoryOrder order)
		{
			if (order.state != QuikSharp.DataStructures.State.Completed) return;
			HystoryOrder newOrder = new(order);
			newOrder.transID = order.transID;
			//newOrder.state = QuikSharp.DataStructures.State.Active;
			Orders.CreateOrder(newOrder);
		}
		public long GetNewTransID() => ++lastTransID;
		private long lastTransID = 0;
	}

	abstract class AbstractOrdersContainer : IOrderFunctions<HystoryOrder>, IHystoryOrderEvents, ITickProcessor
	{
		public event Action<HystoryOrder> OrderChanged;


		protected List<HystoryOrder> actives = new();
		List<HystoryOrder> dones = new();
		List<HystoryOrder> awaitingActivation = new();
		Func<long> GetNewTransId;

		public AbstractOrdersContainer(Func<long> getNewTransId)
		{
			GetNewTransId = getNewTransId;
		}

		public void OnNewTick(Tick tick)
		{
			ActivateAllAwaiting();
			for (int i = actives.Count - 1; i > -1; i--)
			{
				if (actives[i].state == QuikSharp.DataStructures.State.Canceled)
				{
					MoveOrderAtIndexToDones(i);
					continue;
				}
				if (NeedExecution(actives[i], tick.PRICE))
				{
					ExecuteOrderAt(i, tick.PRICE);
					continue;
				}
			}
		}
		

		public Task<long> CreateOrder(HystoryOrder order)
		{
			if (order.state != QuikSharp.DataStructures.State.Active) 
				return Task.FromResult(-1L);
			if(order.transID == 0)
				order.transID = GetNewTransId();
			awaitingActivation.Add(order);
			return Task.FromResult(order.transID);
		}
		public Task<HystoryOrder?> GetOrder_by_transID(HystoryOrder order)
		{
			return Task.FromResult(actives.Find((active) => active.transID == order.transID)) ??
					Task.FromResult(dones.Find((active) => active.transID == order.transID));
		}
		public Task<long> KillOrder(HystoryOrder order)
		{
			if (order.state != QuikSharp.DataStructures.State.Active) return Task.FromResult(-1L);
			order.state = QuikSharp.DataStructures.State.Canceled;
			return Task.FromResult(order.transID);
		}
		protected void ActivateOrderAt(int i)
		{
			HystoryOrder order = awaitingActivation[i];
			awaitingActivation.RemoveAt(i);
			actives.Add(order);
			OrderChanged?.Invoke(order);
		}
		protected void ExecuteOrderAt(int index, double price)
		{
			actives[index].completionPrice = price;
			actives[index].state = QuikSharp.DataStructures.State.Completed;
			MoveOrderAtIndexToDones(index);
		}
		protected void MoveOrderAtIndexToDones(int index)
		{
			HystoryOrder order = actives[index];
			actives.RemoveAt(index);
			dones.Add(order);
			OrderChanged?.Invoke(order);
		}
		protected void ActivateAllAwaiting()
		{
			for (int i = awaitingActivation.Count - 1; i > -1; i--)
			{
				ActivateOrderAt(i);
			}
		}
		protected abstract bool NeedExecution(HystoryOrder order, double price);
	}

	class OrdersContainer : AbstractOrdersContainer
	{
		public OrdersContainer(Func<long> GetNewTransId) : base(GetNewTransId) { }
		protected override bool NeedExecution(HystoryOrder order, double price)
		{
			return (order.direction == TradeDirection.buy && order.price >= price) ||
					(order.direction == TradeDirection.sell && order.price <= price);
		}
	}

	class StopsContainer : AbstractOrdersContainer
	{
		public StopsContainer(Func<long> getNewTransId) : base(getNewTransId) { }

		protected override bool NeedExecution(HystoryOrder order, double price)
		{
			return (order.direction == TradeDirection.buy && order.price <= price) ||
					(order.direction == TradeDirection.sell && order.price >= price);
		}
	}
}
