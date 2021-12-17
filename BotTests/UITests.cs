using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.UI;
using RansacsRealTime;

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
	}
}
