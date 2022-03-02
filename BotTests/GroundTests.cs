using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot.Assemblies;
using RansacBot.Ground;
using RansacBot.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTests
{
	[TestClass]
	public class GroundTests
	{
		[TestMethod]
		public void LinearTradeFilterTest()
		{
			LinearTradeFilter filters = new();
			(filters as IItemFilter<Trade>).Subscribe(PrintTrade);
			filters.OnNewTrade(new(100, RansacBot.Trading.TradeDirection.buy));
			(filters as IItemFilter<Trade>).Processor(new(100, 0));
		}
		void PrintTrade(Trade trade)
		{
			Console.WriteLine(trade.price);
		}
	}
}
