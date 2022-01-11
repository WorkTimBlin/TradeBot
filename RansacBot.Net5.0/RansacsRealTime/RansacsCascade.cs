using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RansacsRealTime
{
	public class RansacsCascade
	{
		private readonly List<LevelOfRansacs> levels = new();
		private Vertexes vertexes = new();
		private double percentile;
		public SigmaType TypeOfSigma { get; private set; }
		public int MaxLevel { get; private set; } = 10;
		public bool IsMaxLevelReached { get { return levels.Count == MaxLevel; } }
		public int LevelsCount { get { return levels.Count; } }

		#region Конструкторы
		public RansacsCascade(Vertexes vertexes, SigmaType typeSigma, double percentile = 90)
		{
			SetVertexes(vertexes);
			this.TypeOfSigma = typeSigma;
			this.percentile = percentile;
			OnNewVertexChooser = OnNewVertex0;
		}
		public RansacsCascade(Vertexes vertexes, SigmaType typeSigma, int maxLevels, double percentile = 90)
		{
			SetVertexes(vertexes);
			this.TypeOfSigma = typeSigma;
			MaxLevel = maxLevels;
			OnNewVertexChooser = OnNewVertex0;
			this.percentile = percentile;
		}
		public RansacsCascade(Vertexes vertexes, SigmaType typeSigma, string path)
		{
			SetVertexes(vertexes);
			OnNewVertexChooser = OnNewVertex1;
			path += @"\Hystory" + typeSigma.ToString();
			LoadMetadata(path);
			LoadLevelsStandart(path);
		}
		public RansacsCascade(Vertexes vertexes, string path)
		{
			SetVertexes(vertexes);
			OnNewVertexChooser = OnNewVertex1;
			LoadMetadata(path);
			LoadLevelsStandart(path);
		}
		#endregion

		public delegate void LevelHandler(LevelOfRansacs level);
		public delegate void RansacOnLevelHandler(Ransac ransac, int level);
		public delegate void NewVertexHandler(Tick tick);

		#region events
		public event LevelHandler NewLevel;
		public event RansacOnLevelHandler NewRansac;
		public event RansacOnLevelHandler RebuildRansac;
		public event RansacOnLevelHandler StopRansac;

		public event NewVertexHandler NewVertex;
		#endregion

		private NewVertexHandler OnNewVertexChooser; // chooser of function to handle new vertex

		public Ransac[][] GetAllLevelsAsArrays()
		{
			return levels.Select(x => x.GetRansacsAsArray()).ToArray();
		}
		public LevelOfRansacs GetLevel(int level)
		{
			return levels[level];
		}
		/// <summary>
		/// Warning: deletes the entire directory to save recursevly before writing!
		/// </summary>
		/// <param name="path"></param>
		public void SaveStandart(string path)
		{
			path = path + @"\Hystory" + TypeOfSigma.ToString();

			if (Directory.Exists(path))
				Directory.Delete(path, true);

			Directory.CreateDirectory(path);
			SaveLevels(path);
			SaveMetadata(path);
		}
		public Vertexes GetVertexes()
		{
			return this.vertexes;
		}

		private const string stdMetadataFileName = "metadata.csv";
		private void SaveMetadata(string path, string fileName = stdMetadataFileName)
		{
			using StreamWriter writer = new(path + @"\" + fileName, false, Encoding.UTF8);
			writer.WriteLine("typeSigma;" + TypeOfSigma);
			writer.WriteLine("percentile;" + percentile);
			writer.WriteLine("MaxLevel;" + MaxLevel);
		}
		private void SaveLevels(string path)
		{
			foreach (LevelOfRansacs level in levels)
				level.SaveStandart(path);
		}
		private void LoadMetadata(string path, string fileName = stdMetadataFileName)
		{
			using StreamReader reader = new(path + @"\" + fileName);
			string line = reader.ReadLine().Split(';')[1];
			
			TypeOfSigma = (SigmaType)Enum.Parse(typeof(SigmaType), line);
			percentile = Convert.ToDouble(reader.ReadLine().Split(';')[1]);
			MaxLevel = Convert.ToInt32(reader.ReadLine().Split(';')[1]);
		}
		private void LoadLevelsStandart(string path)
		{
			FileInfo[] files = new DirectoryInfo(path).GetFiles();
			int numberOfLevels = files.Count(x => x.Name.StartsWith("ransacLevel-"));

			for (int i = 0; i < numberOfLevels; i++)
			{
				levels.Add(new(i, path));
				NewVertex += levels[^1].OnNewVertex;
				levels[^1].NewRansacNeed += OnBuildAscHandler;
				levels[^1].RebuildRansacNeed += OnRebuildAscHandler;
			}
		}

		public void OnNewVertex(Tick tick)
		{
			OnNewVertexChooser(tick);
		}

		private void SetVertexes(Vertexes vertexes)
		{
			this.vertexes = vertexes;
			vertexes.cascades.Add(this);
		}
		private void OnNewVertex0(Tick tick)
		{
			NewVertex?.Invoke(tick);
			if (levels.Count < MaxLevel)
				if (vertexes.LastIndexPermited > -1)
				{
					AddNewLevel();
					OnNewVertexChooser = OnNewVertex1;
				}
		}
		private void OnNewVertex1(Tick tick)
		{
			NewVertex?.Invoke(tick);

			if (levels.Count < MaxLevel && levels[^1].LastIndexPermited >= 0)
				AddNewLevel();
		}
		private void AddNewLevel()
		{
			levels.Add(new LevelOfRansacs(levels.Count));
			levels[^1].BuildNewRansac(vertexes.vertexList, 0, TypeOfSigma, percentile);
			NewVertex += levels[^1].OnNewVertex;
			levels[^1].NewRansacNeed += OnBuildAscHandler;
			levels[^1].RebuildRansacNeed += OnRebuildAscHandler;
			levels[^1].StopRansac += OnStopRansac;
			NewLevel?.Invoke(levels[^1]);
			NewRansac?.Invoke(levels[^1].GetRansacs()[^1], levels.Count - 1);
		}
		private void OnRebuildAscHandler(int level, Ransac ransac)
		{
			levels[level].RebuildRansac(
				vertexes.vertexList.GetRange(ransac.firstTickIndex, vertexes.vertexList.Count - ransac.firstTickIndex), 
				ransac.firstTickIndex, 
				TypeOfSigma, 
				percentile);
			RebuildRansac?.Invoke(levels[level].GetRansacs()[^1], level);
		}
		private void OnBuildAscHandler(int level, Ransac lastRansac)
		{
			if (level == 0)
			{
				if (lastRansac.EndTickIndex > vertexes.LastIndexPermited)
					return;
				else
				{
					int startInd = vertexes.GetFirstIndexForNew(lastRansac);
					levels[level].BuildNewRansac(
						vertexes.vertexList.GetRange(startInd, vertexes.vertexList.Count - startInd), 
						startInd, 
						TypeOfSigma, 
						percentile);
					NewRansac?.Invoke(levels[level].GetRansacs()[^1], level);
				}
			}
			else
			{
				if (lastRansac.EndTickIndex > levels[level - 1].LastIndexPermited)
					return;
				else
				{
					int startInd;
					Ransac lastContained = levels[level - 1].GetLastRansacContainedIn(lastRansac);

					if (lastRansac.Slope > 0)
						startInd = vertexes.GetIndexOfMaxTickInRansac(lastContained);
					else
						startInd = vertexes.GetIndexOfMinTickInRansac(lastContained);

					levels[level].BuildNewRansac(
						vertexes.vertexList.GetRange(startInd, vertexes.vertexList.Count - startInd), 
						startInd, 
						TypeOfSigma, 
						percentile);
					NewRansac?.Invoke(levels[level].GetRansacs()[^1], level);
				}
			}
		}
		private void OnStopRansac(int level, Ransac lastRansac)
		{
			StopRansac?.Invoke(lastRansac, level);
		}
	}
}
