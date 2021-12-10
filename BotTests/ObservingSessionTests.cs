using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RansacBot;
using RansacRealTime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace BotTests
{
	[TestClass]
	public class ObservingSessionTests
	{
		class FileFeeder : ITickByInstrumentProvider
		{
			private event TickHandler tickHandler;

			public void FeedAllStandart()
			{
				foreach (Tick tick in Materials.ticks)
				{
					tickHandler.Invoke(tick);
				}
			}

			public void FeedRangeOfStandart(int startIndex, int count)
			{
				for (int i = startIndex; i < startIndex + count; i++)
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
		public void Instantiate()
		{
			ObservingSession orig = InstantiateStandartSession();
		}
		[TestMethod]
		public void FeedAllFile()
		{
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
		}
		[TestMethod]
		public void SaveLoadEmpty()
		{
			Materials.ClearTestSavesFolder();
			ObservingSession orig = InstantiateStandartSession();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			string one = JsonConvert.SerializeObject(orig);
			string two = JsonConvert.SerializeObject(loaded);
			Assert.AreEqual(one, two);
		}
		[TestMethod]
		public void SaveLoadFeeded()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);

			ObservingSession loaded = new(Materials.PathForTestSaves);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsErrorTreshold()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.AddNewRansacsCascade(TypeSigma.ErrorThreshold);
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsSigma()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.AddNewRansacsCascade(TypeSigma.Sigma);
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsSigmaInliers()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.AddNewRansacsCascade(TypeSigma.SigmaInliers);
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsConfidenceInterval()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.AddNewRansacsCascade(TypeSigma.СonfidenceInterval);
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsAllFour()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.AddNewRansacsCascade(TypeSigma.ErrorThreshold);
			orig.AddNewRansacsCascade(TypeSigma.Sigma);
			orig.AddNewRansacsCascade(TypeSigma.SigmaInliers);
			orig.AddNewRansacsCascade(TypeSigma.СonfidenceInterval);
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			string data = JsonConvert.SerializeObject(orig);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveFeededLoadContinueFeedingErrorTreshold()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			orig.AddNewRansacsCascade(TypeSigma.ErrorThreshold);
			orig.SubscribeTo(fileFeeder);
			fileFeeder.FeedRangeOfStandart(0, 200000);
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves);
			loaded.UpdateEmpty();
			loaded.SubscribeTo(fileFeeder);
			fileFeeder.FeedRangeOfStandart(200000, 200000);
			Assert.AreEqual(orig.ransacs.vertexes.cascades[0].LevelsCount, loaded.ransacs.vertexes.cascades[0].LevelsCount);
		}
		[TestMethod]
		public void FeedPartThenFeedNextAsIfItWasGap()
		{
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession();
			ObservingSession gapFilled = InstantiateStandartSession();
			orig.SubscribeTo(fileFeeder);
			gapFilled.SubscribeTo(fileFeeder);
			fileFeeder.FeedRangeOfStandart(0, 200000);
			gapFilled.UnsubscribeOf(fileFeeder);
			fileFeeder.FeedRangeOfStandart(200000, Materials.ticks.Count - 200000);
			gapFilled.UpdateFromTicksUpToEnd(Materials.ticks);
			//Task task = Task.Run(() => gapFilled.UpdateFromTicksUpToEndKeepingUpWithProviderWaitingForTime(Materials.ticks, fileFeeder, new System.TimeSpan(0, 0, 5)));
			//Task.Run(() => fileFeeder.FeedRangeOfStandart(300000, 100000));
			//task.Wait();
			Assert.AreEqual(JsonConvert.SerializeObject(orig.ransacs), JsonConvert.SerializeObject(gapFilled.ransacs));
		}
	}
}
