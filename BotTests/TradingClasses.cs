using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuikSharp.DataStructures;
//using QuikSharp.DataStructures.Transaction;
using RansacBot;
using RansacBot.QuikRelated;
using RansacBot.Trading;

namespace BotTests
{
	[TestClass]
	public class TradingClasses
	{
		TradeParams tradeParamsDemo = new("SPBFUT", "RIH2", "SPBFUT0067Y", "50290");
		TradeParams tradeParamsReal = new("SPBFUT", "RIH2", "SPBFUTL81pa", "SPBFUTL81pa");
		//[TestMethod]
		public void TestStopStorage()
		{
			StopStorageClassic stopStorage = new(tradeParamsDemo);
			stopStorage.OnNewTradeWithStop(new(new(0, TradeDirection.buy), 0));
			Task.Delay(100).Wait();
			stopStorage.ClosePercentOfLongs(100);
		}
		//[TestMethod]
		public void TestCheckpointExecuted()
		{
			TradeWithStop trade = null;
			StopStorageClassic stopStorage = new(tradeParamsDemo);
			QuikOneOrderAtATimeCheckpoint checkpoint = new(tradeParamsDemo);
			checkpoint.NewTradeWithStop += (tradeW) => 
			{ 
				trade = tradeW;
			};
			checkpoint.OnNewTradeWithStop(new(new(157600, RansacBot.Trading.TradeDirection.buy), 156000));
			Assert.IsTrue(Task.Run(() => { while (trade == null) ; }).Wait(500));
			Assert.AreNotEqual(157600, trade.price);
		}
		//[TestMethod]
		public void TestCheckpointKill()
		{
			TradeParams tradeParams = tradeParamsDemo;
			TradeWithStop trade = null;
			StopStorageClassic stopStorage = new(tradeParams);
			QuikOneOrderAtATimeCheckpoint checkpoint = new(tradeParams);
			checkpoint.NewTradeWithStop += (tradeW) => { trade = tradeW; };
			checkpoint.OnNewTradeWithStop(new(new(152000, TradeDirection.buy), 150000));
			Task.Delay(1500).Wait();
			checkpoint.OnNewTradeWithStop(new(new(152000, TradeDirection.buy), 150000));
			Task.Delay(5000).Wait();
			checkpoint.OnNewTradeWithStop(new(new(156000, TradeDirection.buy), 150000));
			Task.Delay(1500).Wait();
			//Assert.IsTrue(Task.Run(() => { while (trade == null) ; }).Wait(1000));
			//debil
		}
		//[TestMethod]
		public void TestQuikFunctions()
		{
			//Order order = QuikContainer.Quik.Orders.SendLimitOrder(
			//	tradeParams.classCode,
			//	tradeParams.secCode,
			//	tradeParams.accountId,
			//	Operation.Buy,
			//	155000,
			//	1).Result;
			//Console.WriteLine(order.OrderNum);
			List<QuikSharp.DataStructures.Transaction.Order> orders = QuikContainer.Quik.Orders.GetOrders().Result;
		}
		[TestMethod]
		public void TestStopsOperator()
		{
			SimpleStopsOperator stopsOperator = new();
			TradeWithStop sentTradeToComplete = new(new(1000, TradeDirection.buy), 0);
			TradeWithStop sentTradeToKill = new(new(100, TradeDirection.buy), 0);
			stopsOperator.OnNewTradeWithStop(sentTradeToComplete);
			TradeWithStop recievedStop = null;

			stopsOperator.StopExecuted += (tradeWithStop, price) =>
			{
				Assert.AreEqual(sentTradeToComplete, tradeWithStop);
				Assert.AreEqual(price, tradeWithStop.stop.price);
				recievedStop = tradeWithStop;
			};

			SimpleEnsurersController.CompleteAllStops();

			Assert.AreEqual(sentTradeToComplete, recievedStop);

			stopsOperator.UnexecutedStopRemoved += (trade) =>
			{
				Assert.AreEqual(sentTradeToKill, trade);
				recievedStop = trade;
			};

			stopsOperator.OnNewTradeWithStop(sentTradeToKill);
			SimpleEnsurersController.KillAllStops();

			Assert.AreEqual(sentTradeToKill, recievedStop);
		}
		class SimpleEnsurersController
		{
			public static Action CompleteAllTrades;
			public static Action CompleteAllStops;
			public static Action KillAllStops;
		}
		class SimpleStopsOperator : AbstractClassicStopsOperator<TradeWithStop, RansacBot.Trading.Trade>
		{
			public SimpleStopsOperator() : base(
				(trade1, trade2) => trade1 == trade2 ? 0 : (trade1.Order.price > trade2.Order.price ? 1 : -1),
				(trade1, trade2) => trade1 == trade2 ? 0 : (trade1.Order.price > trade2.Order.price ? -1 : 1))
			{ }

			public override string GetSerialized(AbstractStopOrderEnsurer<TradeWithStop, Trade> stopOrder)
			{
				return stopOrder.Order.price.ToString();
			}

			public override string GetSerialized(AbstractOrderEnsurerWithPrice<Trade> ensurer)
			{
				return ensurer.Order.price.ToString();
			}

			protected override AbstractStopOrderEnsurer<TradeWithStop, RansacBot.Trading.Trade> 
				BuildStopOrderEnsurer(TradeWithStop trade)
			{
				return new SimpleStopEnsurer(trade);
			}

			protected override AbstractOrderEnsurerWithPrice<RansacBot.Trading.Trade> 
				GetOrderEnsurer(RansacBot.Trading.Trade order)
			{
				return new SimpleOrderEnsurer(order);
			}

			protected override bool IsLong(AbstractStopOrderEnsurer<TradeWithStop, Trade> stopEnsurer)
			{
				return stopEnsurer.Order.direction == TradeDirection.buy;
			}
		}
		class SimpleStopEnsurer : AbstractStopOrderEnsurer<TradeWithStop, Trade>
		{
			public SimpleStopEnsurer(TradeWithStop tradeWithStop) : 
				base(tradeWithStop, TradeWithStopFuncs<TradeWithStop>.Instance)
			{
				SimpleEnsurersController.CompleteAllStops += CompleteThis;
				SimpleEnsurersController.KillAllStops += OnKilled;
			}

			public void CompleteThis()
			{
				OnCompleted();
			}

			protected override Trade GetCompletionAttribute()
			{
				return Order.stop;
			}
			protected override State GetState(TradeWithStop order)
			{
				throw new NotImplementedException();
			}
			protected override bool IsTransIDMatching(TradeWithStop order)
			{
				throw new NotImplementedException();
			}
			protected override void SetTransID(long transID)
			{
				//throw new NotImplementedException();
			}
			protected override void SubscribeToOnOrderEvent() { }
			protected override void UnsubscribeFromOnOrderEvent()
			{
				throw new NotImplementedException();
			}
		}
		class SimpleOrderEnsurer : AbstractOrderEnsurerWithPrice<Trade>
		{
			public SimpleOrderEnsurer(Trade tradeWithStop) : base(tradeWithStop, TradeWithStopFuncs<Trade>.Instance)
			{
				SimpleEnsurersController.CompleteAllTrades += CompleteThis;
			}

			public void CompleteThis()
			{
				OnCompleted();
			}

			protected override double GetCompletionAttribute()
			{
				return Order.price;
			}

			protected override State GetState(Trade order)
			{
				return QuikSharp.DataStructures.State.Completed;
			}

			protected override bool IsTransIDMatching(Trade order)
			{
				return order == Order;
			}

			protected override void SetTransID(long transID)
			{
				throw new NotImplementedException();
			}

			protected override void SubscribeToOnOrderEvent()
			{
				throw new NotImplementedException();
			}

			protected override void UnsubscribeFromOnOrderEvent() { }

		}
		class TradeWithStopFuncs<TTrade> : IOrderFunctions<TTrade>
		{
			public static TradeWithStopFuncs<TTrade> Instance { get; } = new();
			private TradeWithStopFuncs() { }

			public Task<long> CreateOrder(TTrade order)
			{
				return Task.FromResult(1L);
			}

			public Task<TTrade> GetOrder_by_transID(TTrade order)
			{
				return Task.FromResult(order);
			}

			public Task<long> KillOrder(TTrade order)
			{
				throw new NotImplementedException();
			}
		}
	}
}
