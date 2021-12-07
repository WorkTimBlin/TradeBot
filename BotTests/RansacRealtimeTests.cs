using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacRealTime;
using RansacBot;

namespace BotTests
{
	[TestClass]
	public class RansacRealtimeTests
	{
		public string testFilesPath = @"F:\tim\foderForTests";

		[TestMethod]
		public void TestMonkeyNSaveLoad()
		{
			//Arrange
			MonkeyNFilter first = new(100);
			TicksFromFinamHystory ticks = new(new System.DateTime(2021, 12, 1), new System.DateTime(2021, 12, 1));//used for feeding

			//feeding with ticks of 01.12.2012 from finam
			foreach(Tick tick in ticks)
			{
				first.OnNewTick(tick);
			}
			//saving
			first.SaveStandart(testFilesPath);
			//loading into new
			MonkeyNFilter loaded = new(testFilesPath);

			//Assert
			Assert.AreEqual(first, loaded, "feeded and loaded are not equal!");
		}
	}
}
