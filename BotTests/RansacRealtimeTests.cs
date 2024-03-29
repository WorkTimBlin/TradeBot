using Microsoft.VisualStudio.TestTools.UnitTesting;
using RansacsRealTime;
using RansacBot;
using System.IO;
using System;

namespace BotTests
{
	static class Materials
	{
		public static readonly string PathForTestSaves = Directory.GetCurrentDirectory() + @"\TestsProperties\TestSaves";
		public static readonly string FilesForTestingPath = Directory.GetCurrentDirectory() + @"\TestsProperties\FolderForTests";
		public static TicksLazyParser ticks = new(
					File.ReadAllText(FilesForTestingPath + @"/1.txt").
					Split("\r\n", StringSplitOptions.RemoveEmptyEntries));//used for feeding
		public static void ClearTestSavesFolder()
		{
			Directory.Delete(PathForTestSaves, true);
			Directory.CreateDirectory(PathForTestSaves);
		}
	}

	public class RansacRealtimeTests
	{
		public static string PathForTestSaves = Materials.PathForTestSaves;
		public static string FilesForTestingPath = Materials.FilesForTestingPath;
		public static TicksLazyParser ticks = Materials.ticks;

		[TestClass]
		public class MonkeyNFilterTests
		{
			[TestMethod]
			public void EqualsEmpty()
			{
				MonkeyNFinder first = new(100);
				MonkeyNFinder second = new(100);
				bool bob = first.Equals(second);
				Assert.IsTrue(first.Equals(second), "equal objects are not equal");
			}

			[TestMethod]
			public void EqualsSelf()
			{
				MonkeyNFinder filter = new(1);
				Assert.IsTrue(filter.Equals(filter), "object is not equal to itself!");
			}

			[TestMethod]
			public void SaveLoad()
			{
				//Arrange
				MonkeyNFinder first = new(100);
				//feeding with ticks of 01.12.2012 from finam
				foreach (Tick tick in ticks)
				{
					first.OnNewTick(tick);
				}
				//saving
				first.SaveStandart(PathForTestSaves);
				//loading into new
				MonkeyNFinder loaded = new(PathForTestSaves);

				//Assert
				Assert.IsTrue(first.Equals(loaded), "feeded and loaded are not equal!");
			}
		}

		[TestClass]
		public class RansacSessionTests
		{

			[TestMethod]
			public void Saveload()
			{
				RansacsSession session = new(100);
				RansacsCascade cascade = new(session.vertexes, SigmaType.ErrorThreshold);
				foreach (Tick tick in ticks)
				{
					session.OnNewTick(tick);
				}
				session.SaveStandart(PathForTestSaves);
				RansacsSession loaded = new(PathForTestSaves, loadCascades: true);
				for (int i = 0; i < session.vertexes.vertexList.Count; i++)
				{
					Assert.AreEqual(session.vertexes.vertexList[i], loaded.vertexes.vertexList[i], i.ToString() + "th ticks aren't equal!");
				}
			}
		}
	}
}
