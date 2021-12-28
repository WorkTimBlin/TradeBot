using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.Trading;
using RansacsRealTime;
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
	}
}
