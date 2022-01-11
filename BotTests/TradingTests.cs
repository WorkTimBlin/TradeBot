using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.Trading;
using RansacsRealTime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		[TestMethod]
		public void MaximinStopPlacer()
		{
			MaximinStopPlacer stopPlacer = new(new RansacsCascade(new Vertexes(), SigmaType.ErrorThreshold), 0);
			stopPlacer.OnNewRansac(new Ransac(0, 0, 0, 0, 1, 0, 0, 0), 0);
			TradeWithStop recievedTradeWithStop = new(new(0, TradeDirection.buy), 0);
			stopPlacer.NewTradeWithStop += (TradeWithStop trade) => { recievedTradeWithStop = trade; };
			stopPlacer.OnNewTrade(new(200, TradeDirection.buy));
			//TODO finish
		}
	}
}
