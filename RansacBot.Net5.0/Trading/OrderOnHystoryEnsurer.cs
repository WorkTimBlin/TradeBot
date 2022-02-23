using QuikSharp.DataStructures;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	class OrderOnHystoryEnsurer : AbstractOrderEnsurerWithPrice<HystoryOrder>
	{
		public OrderOnHystoryEnsurer(HystoryOrder trade):base(trade, HystoryInfra.Instance) { }

		protected override double GetCompletionAttribute()
		{
			return Order.completionPrice;
		}

		protected override State GetState(HystoryOrder order)
		{
			return order.state;
		}

		//don't need this funcs because order is updating itself
		
		protected override bool IsTransIDMatching(HystoryOrder order) => false;
		protected override void SetTransID(long transID) { }
		protected override void SubscribeToOnOrderEvent() { }
		protected override void UnsubscribeFromOnOrderEvent() { }
	}

	class HystoryInfra : IOrderFunctions<HystoryOrder>, ITickFilter
	{
		public static HystoryInfra Instance { get; private set; } = new();
		List<HystoryOrder> done = new();
		List<HystoryOrder> actives = new();

		public event Action<Tick> NewTick;

		private HystoryInfra() { }

		public static void Clear()
		{
			Instance = new();
		}

		public async Task<long> CreateOrder(HystoryOrder order)
		{
			actives.Add(order);
			return order.transID;
		}

		/// <summary>
		/// just returns the order from args.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		public async Task<HystoryOrder?> GetOrder_by_transID(HystoryOrder order)
		{
			return order;
		}

		public async Task<long> KillOrder(HystoryOrder order)
		{
			actives.Remove(order);
			done.Add(order);
			order.state = State.Canceled;
			return 1;
		}

		public void OnNewTick(Tick tick)
		{
			for(int i = 0; i < actives.Count; i++)
			{
				if(ShouldExecuteOrderOnPrice(actives[i], tick.PRICE))
				{
					ExecuteOrderAtPriceInActivesAt(tick.PRICE, i);
					i--;
				}
			}
			NewTick?.Invoke(tick);
		}
		private void ExecuteOrderAtPriceInActivesAt(double price, int i)
		{
			HystoryOrder order = actives[i];
			order.completionPrice = price;
			done.Add(order);
			actives.RemoveAt(i);
			order.state = State.Completed;
		}
		private bool ShouldExecuteOrderOnPrice(HystoryOrder order, double price)
		{
			return order.trade.direction == TradeDirection.buy && order.completionPrice >= price ||
				order.trade.direction == TradeDirection.sell && order.completionPrice <= price;
		}
	}

	class HystoryStops : ITickFilter, IStopsOperator
	{
		public event Action<Tick> NewTick;
		public event Action<TradeWithStop, double> StopExecuted;
		public event Action<TradeWithStop> UnexecutedStopRemoved;

		public void ClosePercentOfLongs(double percent)
		{
			throw new NotImplementedException();
		}

		public void ClosePercentOfShorts(double percent)
		{
			throw new NotImplementedException();
		}

		public void OnNewTick(Tick tick)
		{
			throw new NotImplementedException();
		}

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			throw new NotImplementedException();
		}
	}
}
