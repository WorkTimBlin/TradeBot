using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.UI;

namespace BotTests
{
	[TestClass]
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
				ransacs.Add(new(0, 10, 20, 30, 50, 50, 10, 10));
			}
			[TestMethod]
			public void AddOneFalling()
			{
				RansacColorfulSeries ransacs = new();
				ransacs.Add(new(0, 10, 20, 30, -50, 50, 10, 10));
			}
			[TestMethod]
			public void AddMillionOfRaisingAndFallingEach()
			{
				RansacColorfulSeries ransacs = new();
				for(int i = 0; i < 1000000; i++)
				{
					ransacs.Add(new(i * 10, i * 10, i * 10, 10, i, i, 1f / i, 1f / i));
				}
				for(int i = 0; i < 1000000; i++)
				{
					ransacs.Add(new(i * 10, i * 10, i * 10, 10, -i, i, 1f / i, 1f / i));
				}
				Assert.AreEqual(2000000, ransacs.Count);
			}
		} 
	}
}
