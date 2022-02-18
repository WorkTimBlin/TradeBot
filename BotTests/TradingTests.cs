﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.Trading;
using RansacsRealTime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacBot;
using QuikSharp;
using QuikSharp.DataStructures;
using RansacBot.QuikRelated;

namespace BotTests
{
	[TestClass]
	public class TradingTests
	{
		[TestMethod]
		public void HigherLowerFilter()
		{
			HigherLowerFilter filter = new();
			bool pass = false;
			filter.NewExtremum += (Tick tick, VertexType type, Tick current) => { pass = true; };
			filter.OnNewExtremum(new(0, 0, 150), VertexType.Low, new(0, 0, 250));
			Assert.AreEqual(false, pass);
			filter.OnNewExtremum(new(0, 0, 300), VertexType.High, new(0, 0, 200));
			Assert.AreEqual(false, pass);
			filter.OnNewExtremum(new(0, 0, 160), VertexType.Low, new(0, 0, 260));
			Assert.AreEqual(false, pass);
			filter.OnNewExtremum(new(0, 0, 310), VertexType.High, new());
			Assert.AreEqual(true, pass);
			pass = false;
			filter.OnNewExtremum(new(0, 0, 150), VertexType.Low, new());
			Assert.AreEqual(true, pass);
			pass = false;
			filter.OnNewExtremum(new(0, 0, 300), VertexType.High, new());
			Assert.AreEqual(false, pass);
		}
		[TestMethod]
		public void InvertedNDecider()
		{
			InvertedNDecider decider = new();
			Trade recievedTrade = new(0, TradeDirection.buy);
			decider.NewTrade += (Trade trade) => { recievedTrade = trade; };
			decider.OnNewExtremum(new(0, 0, 100), VertexType.Low, new(0, 0, 200));
			Assert.AreEqual(JsonConvert.SerializeObject(new Trade(200, TradeDirection.sell)), JsonConvert.SerializeObject(recievedTrade));
			recievedTrade = new(0, TradeDirection.buy);
			decider.OnNewVertex(new(), VertexType.Low);
			Assert.AreEqual(JsonConvert.SerializeObject(new Trade(0, TradeDirection.buy)), JsonConvert.SerializeObject(recievedTrade));
			decider.OnNewExtremum(new(0, 0, 300), VertexType.High, new(0, 0, 200));
			Assert.AreEqual(JsonConvert.SerializeObject(new Trade(200, TradeDirection.buy)), JsonConvert.SerializeObject(recievedTrade));
		}
		MaximinStopPlacer GetTestMaximinStopPlacerInstance()
		{
			Vertexes vertexes = new();
			vertexes.OnNewVertex(new(0, 0, 100));
			vertexes.OnNewVertex(new(0, 1, 500));
			vertexes.OnNewVertex(new(0, 2, 200));
			vertexes.OnNewVertex(new(0, 3, 600));
			return new(new RansacsCascade(vertexes, SigmaType.ErrorThreshold), 0);
		}
		[TestMethod]
		public void MaximinStopPlacerNoRansac()
		{
			MaximinStopPlacer stopPlacer = GetTestMaximinStopPlacerInstance();
			TradeWithStop recievedTradeWithStop = new(new(0, TradeDirection.buy), 0);
			stopPlacer.NewTradeWithStop += (TradeWithStop trade) => { recievedTradeWithStop = trade; };
			stopPlacer.OnNewTrade(new(200, TradeDirection.buy));
			Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(0, TradeDirection.buy), 0)), JsonConvert.SerializeObject(recievedTradeWithStop));
		}
		//[TestMethod]
		//public void MaximinStopPlacerFirstRansacOnly()
		//{
		//	MaximinStopPlacer stopPlacer = GetTestMaximinStopPlacerInstance();
		//	TradeWithStop recievedTradeWithStop = new(new(0, TradeDirection.buy), 0);
		//	stopPlacer.NewTradeWithStop += (TradeWithStop trade) => { recievedTradeWithStop = trade; };

		//	stopPlacer.OnNewRansac(new Ransac(0, 0, 0, 2, 1, 0, 0, 0), 0);
		//	stopPlacer.OnNewTrade(new(200, TradeDirection.buy));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(200, TradeDirection.buy), 100)), JsonConvert.SerializeObject(recievedTradeWithStop));
		//	recievedTradeWithStop = new(new(0, TradeDirection.buy), 0);
		//	stopPlacer.OnNewTrade(new(300, TradeDirection.sell));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(0, TradeDirection.buy), 0)), JsonConvert.SerializeObject(recievedTradeWithStop));

		//	stopPlacer.OnRebuildRansac(new Ransac(0, 0, 0, 2, -1, 0, 0, 0), 0);
		//	recievedTradeWithStop = new(new(0, TradeDirection.buy), 0);
		//	stopPlacer.OnNewTrade(new(300, TradeDirection.sell));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(300, TradeDirection.sell), 500)), JsonConvert.SerializeObject(recievedTradeWithStop));
		//	recievedTradeWithStop = new(new(0, TradeDirection.sell), 0);
		//	stopPlacer.OnNewTrade(new(200, TradeDirection.buy));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(0, TradeDirection.sell), 0)), JsonConvert.SerializeObject(recievedTradeWithStop));
		//}
		//[TestMethod]
		//public void MaximinStopPlacerSecondRansacOnlyWithFirstRaising()
		//{
		//	MaximinStopPlacer stopPlacer = GetTestMaximinStopPlacerInstance();
		//	TradeWithStop recievedTradeWithStop = new(new(0, TradeDirection.buy), 0);
		//	stopPlacer.NewTradeWithStop += (TradeWithStop trade) => { recievedTradeWithStop = trade; };

		//	stopPlacer.OnNewRansac(new Ransac(0, 0, 0, 2, 1, 0, 0, 0), 0);
		//	stopPlacer.OnStopRansac(new Ransac(0, 0, 0, 2, 1, 0, 0, 0), 0);
		//	stopPlacer.OnNewTrade(new(200, TradeDirection.buy));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(200, TradeDirection.buy), 100)), JsonConvert.SerializeObject(recievedTradeWithStop));
		//	recievedTradeWithStop = new(new(0, TradeDirection.buy), 0);
		//	stopPlacer.OnNewTrade(new(300, TradeDirection.sell));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(300, TradeDirection.sell), 500)), JsonConvert.SerializeObject(recievedTradeWithStop));

		//	stopPlacer.OnNewRansac(new Ransac(2, 2, 2, 2, 1, 0, 0, 0), 0);
		//	stopPlacer.OnNewTrade(new(300, TradeDirection.buy));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(300, TradeDirection.buy), 200)), JsonConvert.SerializeObject(recievedTradeWithStop));
		//	stopPlacer.OnNewTrade(new(400, TradeDirection.sell));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(400, TradeDirection.sell), 500)), JsonConvert.SerializeObject(recievedTradeWithStop));

		//	stopPlacer.OnRebuildRansac(new Ransac(2, 2, 2, 2, -1, 0, 0, 0), 0);
		//	stopPlacer.OnNewTrade(new(300, TradeDirection.buy));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(300, TradeDirection.buy), 100)), JsonConvert.SerializeObject(recievedTradeWithStop));
		//	stopPlacer.OnNewTrade(new(400, TradeDirection.sell));
		//	Assert.AreEqual(JsonConvert.SerializeObject(new TradeWithStop(new(400, TradeDirection.sell), 600)), JsonConvert.SerializeObject(recievedTradeWithStop));
		//}

		[TestMethod]
		public void ConnectionCheck()
		{
			QuikTradeConnector quikTradeConnector = new(new("SPBFUT", "RIH2"), "SPBFUT005gx");
			quikTradeConnector.OnNewTradeWithStop(new(new(120, TradeDirection.sell), 0));
		}

		[TestMethod]
		public void TradeCallbackCheck()
		{
			Quik quik = QuikContainer.Quik;
			double price = quik.Trading.GetAllTrades().Result[^1].Price;
			QuikTradeConnector quikTradeConnector = new(new("SPBFUT", "RIH2"), "SPBFUT005gx");
			bool gotCallback = false;
			quikTradeConnector.NewTradeWithStop += (TradeWithStop trade) => { gotCallback = true; };
			quikTradeConnector.OnNewTradeWithStop(new(new(price, TradeDirection.buy), 0));
			Task.Delay(5000).Wait();
			while (!gotCallback) ;
			if (!gotCallback) throw new Exception("didn't recieve callbeck!");
		}

		[TestMethod]
		public void SendStopAndKillUsingTradeConnector()
		{
			QuikTradeConnector tradeConnector = new(new("SPBFUT", "RIH2"), "SPBFUT005gx");
			tradeConnector.OnNewTradeWithStop(new(new(155000, TradeDirection.buy), 148260));
			tradeConnector.OnNewTradeWithStop(new(new(156110, TradeDirection.buy), 148260));
			tradeConnector.ClosePercentOfLongs(100);
			if (!tradeConnector.IsLastOrderExecuted) throw new Exception("unexecuted order");
			//while (true) ;
		}
		//[TestMethod]
		//public void SendUnrealisticTradeAndAnotherAndClose()
		//{
		//	bool recieved = false;
		//	Task.Run(() =>
		//	{
		//		TradeParams tradeParams = new("SPBFUT", "RIH2", "SPBFUT005gx", null);
		//		StopStorage stopStorage = new(QuikContainer.Quik, tradeParams);
		//		TradeWithStopEnsurer ensurer = new(QuikContainer.Quik, tradeParams, stopStorage);
		//		QuikContainer.Quik.Events.OnOrder += (QuikSharp.DataStructures.Transaction.Order order) => { recieved = true; };
		//		//ensurer.OnNewTradeWithStop(new(new(150000, TradeDirection.buy), 130000));
		//		ensurer.OnNewTradeWithStop(new(new(154000, TradeDirection.buy), 130000));
		//		Task.Run(() =>
		//		{
		//			Task.Delay(500).Wait();
		//			stopStorage.ClosePercentOfLongs(100);
		//		}).Start();
		//	});
		//	Task.Delay(700).Wait();
		//	Console.WriteLine(recieved.ToString());
		//}
	}
}
