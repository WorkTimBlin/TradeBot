using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RansacBot;
using RansacsRealTime;
using System.IO;
using System.Threading.Tasks;

namespace BotTests
{
	[TestClass]
	public class ObservingSessionTests
	{
		class FileFeeder : ITickByInstrumentProvider
		{
			private event TickHandler NewTick;

			public void FeedAllStandart()
			{
				foreach (Tick tick in Materials.ticks)
				{
					NewTick.Invoke(tick);
				}
			}
			public void FeedRangeOfStandart(int startIndex, int count)
			{
				for (int i = startIndex; i < startIndex + count; i++)
				{
					NewTick.Invoke(Materials.ticks[i]);
				}
			}

			public void Subscribe(Instrument instrument, TickHandler handler)
			{
				NewTick += handler;
			}
			public void Unsubscribe(Instrument instrument, TickHandler handler)
			{
				NewTick -= handler;
			}
		}

		ObservingSession InstantiateStandartSession(ITickByInstrumentProvider fileFeeder)
		{
			return new(new Instrument("RIZ1", "SPBFUT", "", "", ""), fileFeeder, 100);
		}

		[TestMethod]
		public void Instantiate()
		{
			ObservingSession orig = InstantiateStandartSession(new FileFeeder());
		}
		[TestMethod]
		public void FeedAllFile()
		{
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			orig.SubscribeToProvider();
			fileFeeder.FeedAllStandart();
		}
		[TestMethod]
		public void SaveLoadEmpty()
		{
			Materials.ClearTestSavesFolder();
			
			ObservingSession orig = InstantiateStandartSession(new FileFeeder());
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves, new FileFeeder());
			string one = JsonConvert.SerializeObject(orig);
			string two = JsonConvert.SerializeObject(loaded);
			Assert.AreEqual(one, two);
		}
		[TestMethod]
		public void SaveLoadFeeded()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			orig.SubscribeToProvider();
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);

			ObservingSession loaded = new(Materials.PathForTestSaves, fileFeeder);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsErrorTreshold()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			orig.AddNewRansacsCascade(SigmaType.ErrorThreshold);
			orig.SubscribeToProvider();
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves, fileFeeder);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsSigma()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			orig.AddNewRansacsCascade(SigmaType.Sigma);
			orig.SubscribeToProvider();
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves, fileFeeder);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsSigmaInliers()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			orig.AddNewRansacsCascade(SigmaType.SigmaInliers);
			orig.SubscribeToProvider();
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves, fileFeeder);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsConfidenceInterval()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			orig.AddNewRansacsCascade(SigmaType.СonfidenceInterval);
			orig.SubscribeToProvider();
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves, fileFeeder);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void SaveLoadFeededWithRansacsAllFour()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			orig.AddNewRansacsCascade(SigmaType.ErrorThreshold);
			orig.AddNewRansacsCascade(SigmaType.Sigma);
			orig.AddNewRansacsCascade(SigmaType.SigmaInliers);
			orig.AddNewRansacsCascade(SigmaType.СonfidenceInterval);
			orig.SubscribeToProvider();
			fileFeeder.FeedAllStandart();
			orig.SaveStandart(Materials.PathForTestSaves);
			ObservingSession loaded = new(Materials.PathForTestSaves, fileFeeder);
			string data = JsonConvert.SerializeObject(orig);
			Assert.AreEqual(JsonConvert.SerializeObject(orig), JsonConvert.SerializeObject(loaded));
		}
		[TestMethod]
		public void FeedPartThenFeedNextAsIfItWasGap()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			ObservingSession gapFilled = InstantiateStandartSession(fileFeeder);
			orig.SubscribeToProvider();
			gapFilled.SubscribeToProvider();
			fileFeeder.FeedRangeOfStandart(0, 200000);
			gapFilled.UnsubscribeOfProvider();
			gapFilled.SaveStandart(Materials.PathForTestSaves);
			gapFilled = new(Materials.PathForTestSaves, fileFeeder);
			fileFeeder.FeedRangeOfStandart(200000, Materials.ticks.Count - 200000);
			gapFilled.UpdateFromTicksUpToEnd(Materials.ticks);
			//Task task = Task.Run(() => gapFilled.UpdateFromTicksUpToEndKeepingUpWithProviderWaitingForTime(Materials.ticks, fileFeeder, new System.TimeSpan(0, 0, 5)));
			//Task.Run(() => fileFeeder.FeedRangeOfStandart(300000, 100000));
			//task.Wait();
			Assert.AreEqual(JsonConvert.SerializeObject(orig.ransacs), JsonConvert.SerializeObject(gapFilled.ransacs));
		}
		[TestMethod]
		public void FeedPartSaveLoadFeedGapKeepingUpWithProvider()
		{
			Materials.ClearTestSavesFolder();
			FileFeeder fileFeeder = new();
			ObservingSession orig = InstantiateStandartSession(fileFeeder);
			ObservingSession gapFilled = InstantiateStandartSession(fileFeeder);
			orig.SubscribeToProvider();
			gapFilled.SubscribeToProvider();
			fileFeeder.FeedRangeOfStandart(0, 200000);
			gapFilled.UnsubscribeOfProvider();
			gapFilled.SaveStandart(Materials.PathForTestSaves);
			fileFeeder.FeedRangeOfStandart(200000, 100000);
			gapFilled = new(Materials.PathForTestSaves, fileFeeder);
			Task task = Task.Run(() => gapFilled.UpdateFromTicksUpToEndKeepingUpWithProviderWaitingForTime(Materials.ticks, new System.TimeSpan(0, 0, 5)));
			Task.Run(() => fileFeeder.FeedRangeOfStandart(300000, 100000));
			task.Wait();
			Assert.AreEqual(JsonConvert.SerializeObject(orig.ransacs), JsonConvert.SerializeObject(gapFilled.ransacs));
		}
		//[TestMethod] //uncomment only for reconstructing the dataset test file
		public void SaveHystoryFileInNewLocation()
		{
			using StreamWriter streamWriter = new(Directory.GetCurrentDirectory() + "/TestsProperties/Folder/1.txt");
			string[] HystoryStrings = FinamDataLoader.RawFinamHystory.GetTickLines(new System.DateTime(2021, 12, 1), new System.DateTime(2021, 12, 2));
			for(int i = 0; i < HystoryStrings.Length; i++)
			{
				streamWriter.WriteLine(HystoryStrings[i]);
			}
		}
	}
}
