using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacBot;
using RansacRealTime;


namespace BotTests
{
	[TestClass]
	class ObservingSessionTests
	{
		class FileFeeder : ITickByInstrumentProvider
		{
			private event TickHandler tickHandler;

			public void FeedAllStandart()
			{
				foreach(Tick tick in Materials.ticks)
				{
					tickHandler.Invoke(tick);
				}
			}

			public void FeedRangeOfStandart(int startIndex, int count)
			{
				for(int i = startIndex; i < startIndex + count; i++)
				{
					tickHandler.Invoke(Materials.ticks[i]);
				}
			}

			public void Subscribe(Instrument instrument, TickHandler handler)
			{
				tickHandler += handler;
			}
			public void Unsubscribe(Instrument instrument, TickHandler handler)
			{
				tickHandler -= handler;
			}
		}
		
		ObservingSession InstantiateStandartSession()
		{
			return new(new Instrument("RIZ1", "SPBFUT", "", "", ""), 100);
		}

		[TestMethod]
		void Instantiate()
		{
			ObservingSession orig = InstantiateStandartSession();
		}
		[TestMethod]
		void FeedAllFile()
		{
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
		}
		[TestMethod]
		void SaveLoadEmpty()
		{
			ObservingSession orig = InstantiateStandartSession();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			Assert.AreEqual(orig, loaded);
		}
		[TestMethod]
		void SaveLoadFeeded()
		{
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.SubscribeTo(fileFeeder);
			orig.SaveStandart(Materials.PathForTestSaves);
			
			ObservingSession loaded = new(Materials.PathForTestSaves);
			Assert.AreEqual(orig, loaded);
		}
	}
}
