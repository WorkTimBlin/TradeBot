using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
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
		[TestMethod]
		public void TestStopStorage()
		{
			StopStorageClassic stopStorage = new(tradeParamsDemo);
			stopStorage.OnNewTradeWithStop(new(new(0, TradeDirection.buy), 0));
			Task.Delay(100).Wait();
			stopStorage.ClosePercentOfLongs(100);
		}
		[TestMethod]
		public void TestCheckpointExecuted()
		{
			TradeWithStop? trade = null;
			StopStorageClassic stopStorage = new(tradeParamsDemo);
			OneOrderAtATimeCheckpoint checkpoint = new(tradeParamsDemo);
			checkpoint.NewTradeWithStop += (tradeW) => 
			{ 
				trade = tradeW;
			};
			checkpoint.OnNewTradeWithStop(new(new(157600, RansacBot.Trading.TradeDirection.buy), 156000));
			Assert.IsTrue(Task.Run(() => { while (trade == null) ; }).Wait(500));
			Assert.AreNotEqual(157600, trade.price);
		}
		[TestMethod]
		public void TestCheckpointKill()
		{
			TradeParams tradeParams = tradeParamsDemo;
			TradeWithStop? trade = null;
			StopStorageClassic stopStorage = new(tradeParams);
			OneOrderAtATimeCheckpoint checkpoint = new(tradeParams);
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
		[TestMethod]
		public void TestQuikFunctions()
		{
			//Order order = QuikContainer.Quik.Orders.SendLimitOrder(tradeParams.classCode, tradeParams.secCode, tradeParams.accountId, Operation.Buy, 155000, 1).Result;
			//Console.WriteLine(order.OrderNum);
			List<Order> orders = QuikContainer.Quik.Orders.GetOrders().Result;
		}
	}
}
