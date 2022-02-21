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
	class OrderOnHystoryEnsurer : AbstractOrderEnsurer<TradeOrder>
	{
		public OrderOnHystoryEnsurer(TradeOrder trade):base(trade, HystoryInfra.Instance) { }

		protected override State GetState(TradeOrder order)
		{
			return order.state;
		}

		//don't need this funcs because order is updating itself
		protected override bool IsTransIDMatching(TradeOrder order) => false;
		protected override void SetTransID(long transID) { }
		protected override void SubscribeToOnOrderEvent() { }
		protected override void UnsubscribeFromOnOrderEvent() { }
	}

	class HystoryInfra : IOrderFunctions<TradeOrder>, ITickFilter
	{
		public static HystoryInfra Instance { get; private set; } = new();
		List<TradeOrder> done = new();
		List<TradeOrder> actives = new();

		public event Action<Tick> NewTick;

		private HystoryInfra() { }

		public static void Clear()
		{
			Instance = new();
		}

		public async Task<long> CreateOrder(TradeOrder order)
		{
			actives.Add(order);
			return order.transID;
		}

		/// <summary>
		/// just returns the order from args.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		public async Task<TradeOrder?> GetOrder_by_transID(TradeOrder order)
		{
			return order;
		}

		public async Task<long> KillOrder(TradeOrder order)
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
			TradeOrder order = actives[i];
			order.price = price;
			done.Add(order);
			actives.RemoveAt(i);
			order.state = State.Completed;
		}
		private bool ShouldExecuteOrderOnPrice(TradeOrder order, double price)
		{
			return order.direction == TradeDirection.buy && order.price >= price ||
				order.direction == TradeDirection.sell && order.price <= price;
		}
	}

	class HystoryStops : ITradesHystory, ITickFilter
	{
		public event ClosePosHandler ExecutedLongStop;
		public event ClosePosHandler ExecutedShortStop;
		public event ClosePosHandler KilledLongStop;
		public event ClosePosHandler KilledShortStop;
		public event Action<Tick> NewTick;

		TradesHystory tradesHystory = new();

		HystoryInfra Infra = HystoryInfra.Instance;

		public HystoryStops()
		{
			tradesHystory.ExecutedLongStop += OnLongStop;
			tradesHystory.KilledLongStop += OnLongStop;
			tradesHystory.ExecutedShortStop += OnShortStop;
			tradesHystory.KilledShortStop += OnShortStop;
			tradesHystory.ExecutedLongStop += (price) => ExecutedLongStop?.Invoke(price);
			tradesHystory.KilledLongStop += (price) => KilledLongStop?.Invoke(price);
			tradesHystory.ExecutedShortStop += (price) => ExecutedShortStop?.Invoke(price);
			tradesHystory.KilledShortStop += (price) => KilledShortStop?.Invoke(price);
		}

		public void OnLongStop(decimal price)
		{
			Infra.CreateOrder(new(new(Double.MinValue, TradeDirection.sell))).ConfigureAwait(false);
		}
		public void OnShortStop(decimal price)
		{
			Infra.CreateOrder(new(new(Double.MaxValue, TradeDirection.buy))).ConfigureAwait(false);
		}

		public void ClosePercentOfLongs(double percent)
		{
			tradesHystory.ClosePercentOfLongs(percent);
		}

		public void ClosePercentOfShorts(double percent)
		{
			tradesHystory.ClosePercentOfShorts(percent);
		}

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			tradesHystory.OnNewTradeWithStop(tradeWithStop);
		}

		public void OnNewTick(Tick tick)
		{
			tradesHystory.CheckForStops((decimal)tick.PRICE);
			NewTick?.Invoke(tick);
		}
	}
}
