﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.UI;
using RansacsRealTime;
using RansacBot.Trading;
using OxyPlot.Series;

namespace BotTests
{
	public class UITests
	{
		[TestClass]
		public class RansacsColourfulSeriesTests
		{
			[TestMethod]
			public void Initialise()
			{
				RansacColorfulSeries series = new();
			}
			[TestMethod]
			public void AddOneRaising()
			{
				RansacColorfulSeries ransacs = new();
				ransacs.BuildNewRansac(new(0, 10, 20, 30, 50, 50, 10, 10));
			}
			[TestMethod]
			public void AddOneFalling()
			{
				RansacColorfulSeries ransacs = new();
				ransacs.BuildNewRansac(new(0, 10, 20, 30, -50, 50, 10, 10));
			}
			[TestMethod]
			public void AddMillionOfRaisingAndFallingEach()
			{
				RansacColorfulSeries ransacs = new();
				for(int i = 0; i < 1000000; i++)
				{
					ransacs.BuildNewRansac(new(i * 10, i * 10, i * 10, 10, i, i, 1f / i, 1f / i));
				}
				for(int i = 0; i < 1000000; i++)
				{
					ransacs.BuildNewRansac(new(i * 10, i * 10, i * 10, 10, -i, i, 1f / i, 1f / i));
				}
				Assert.AreEqual(2000000, ransacs.Count);
			}
		} 
		[TestClass]
		public class RansacOxyPrintTests
		{
			[TestMethod]
			public void Initialise()
			{
				RansacsSession session = new(100);
				RansacsCascade cascade = new(session.vertexes, SigmaType.ErrorThreshold);
				RansacsOxyPrinter ransacsPrinter = new(2, cascade);
			}
		}
		[TestClass]
		public class RansacOxyPrintWithTradesTests
        {
			[TestMethod]
			public void AddingLongsWhenExtremumCameFirst()
			{
				//dummy values for ransacPrinterWithTrades initialization
				RansacsSession session = new(100);
				RansacsCascade cascade = new(session.vertexes, SigmaType.ErrorThreshold);
				RansacsOxyPrinterWithTrades ransacPrinterWithTrades = new(2, cascade);
				Random rnd = new Random();

				for (int n = 0; n < 500; n++)
				{
					Tick testExtremumTick = new(0, rnd.Next(1, 1000000), (double)rnd.Next(50000, 200000));
					Tick testCurrentTick = new(0, rnd.Next(1, 1000000), (double)rnd.Next(50000, 200000));
					TradeWithStop testTradeWithStop = new(new Trade(testCurrentTick.PRICE, TradeDirection.buy), testCurrentTick.PRICE - 500);

					ransacPrinterWithTrades.OnNewExtremum(testExtremumTick, VertexType.Low, testCurrentTick);
					ransacPrinterWithTrades.OnNewTradeWithStop(testTradeWithStop);

					Assert.AreEqual(ransacPrinterWithTrades.longs.Points[^1].X, testExtremumTick.VERTEXINDEX + 0.5);
					Assert.AreEqual(ransacPrinterWithTrades.longs.Points[^1].Y, testTradeWithStop.price);
					Assert.AreEqual(ransacPrinterWithTrades.stops.Points[^1].X, testExtremumTick.VERTEXINDEX + 0.5);
					Assert.AreEqual(ransacPrinterWithTrades.stops.Points[^1].Y, testTradeWithStop.price - 500);
				}
			}
			[TestMethod]
			public void AddingShortsWhenTradeCameFirst()
            {
				//dummy values for ransacPrinterWithTrades initialization
				RansacsSession session = new(100);
				RansacsCascade cascade = new(session.vertexes, SigmaType.ErrorThreshold);
				RansacsOxyPrinterWithTrades ransacPrinterWithTrades = new(2, cascade);
				Random rnd = new Random();

				for (int n = 0; n < 500; n++)
				{
					double testPrice = (double)rnd.Next(50000, 200000);
					TradeWithStop testTradeWithStop = new(new Trade(testPrice, TradeDirection.sell), testPrice + 500);
					Tick testExtremumTick = new(0, rnd.Next(1, 1000000), (double)rnd.Next(50000, 200000));
					Tick testCurrentTick = new(0, rnd.Next(1, 1000000), testTradeWithStop.price);

					ransacPrinterWithTrades.OnNewTradeWithStop(testTradeWithStop);
					ransacPrinterWithTrades.OnNewExtremum(testExtremumTick, VertexType.High, testCurrentTick);

					Assert.AreEqual(ransacPrinterWithTrades.shorts.Points[^1].X, testExtremumTick.VERTEXINDEX + 0.5);
					Assert.AreEqual(ransacPrinterWithTrades.shorts.Points[^1].Y, testTradeWithStop.price);
					Assert.AreEqual(ransacPrinterWithTrades.stops.Points[^1].X, testExtremumTick.VERTEXINDEX + 0.5);
					Assert.AreEqual(ransacPrinterWithTrades.stops.Points[^1].Y, testTradeWithStop.price + 500);
				}
			}
        }
	}
}
