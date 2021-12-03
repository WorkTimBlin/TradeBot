using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RansacRealTime
{
	public class RansacsCascade
	{
		private List<LevelOfRansacs> levels = new();
		private Vertexes vertexes = new();
		public TypeSigma typeSigma;
		private double percentile;
		public int MaxLevel { get; private set; } = 10;
		public bool IsMaxLevelReached { get { return levels.Count == MaxLevel; } }
		public int LevelsCount { get { return levels.Count; } }

		#region Конструкторы

		public RansacsCascade(Vertexes vertexes, TypeSigma typeSigma, double percentile = 90)
		{
			SetVertexes(vertexes);
			this.typeSigma = typeSigma;
			this.percentile = percentile;
			OnNewVertexChooser = OnNewVertex0;
		}
		public RansacsCascade(Vertexes vertexes, TypeSigma typeSigma, int maxLevels, double percentile = 90)
		{
			SetVertexes(vertexes);
			this.typeSigma = typeSigma;
			MaxLevel = maxLevels;
			OnNewVertexChooser = OnNewVertex0;
			this.percentile = percentile;
		}
		public RansacsCascade(Vertexes vertexes, TypeSigma typeSigma, string path)
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

		public delegate void NewLevelHandler(LevelOfRansacs level);
		public event NewLevelHandler NewLevel;

		public delegate void RansacHandler(Ransac ransac, int level);
		public event RansacHandler NewRansac;
		public event RansacHandler RebuildRansac;
		public event RansacHandler StopRansac;

		public delegate void NewVertexHandler(Tick tick);
		public event NewVertexHandler NewVertex;

		private delegate void OnNewVertexHandler(Tick tick);
		private OnNewVertexHandler OnNewVertexChooser;


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
			path = path + @"\Hystory" + typeSigma.ToString();

			if (Directory.Exists(path))
				Directory.Delete(path, true);

			Directory.CreateDirectory(path);
			SaveLevels(path);
			SaveMetadata(path);
		}
		private void SaveMetadata(string path)
		{
			using StreamWriter writer = new(path + "/metadata.csv", false, Encoding.UTF8);
			writer.WriteLine("typeSigma;" + typeSigma);
			writer.WriteLine("percentile;" + percentile);
			writer.WriteLine("MaxLevel;" + MaxLevel);
		}
		private void SaveLevels(string path)
		{
			foreach (LevelOfRansacs level in levels)
				level.SaveStandart(path);
		}
		private void LoadMetadata(string path)
		{
			using StreamReader reader = new(path + "/metadata.csv");
			string line = reader.ReadLine().Split(';')[1];
			
			typeSigma = (TypeSigma)Enum.Parse(typeof(TypeSigma), line);
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

		private void SetVertexes(Vertexes vertexes)
		{
			this.vertexes = vertexes;
			vertexes.cascades.Add(this);
		}
		public void OnNewVertex(Tick tick)
		{
			OnNewVertexChooser(tick);
		}
		private void OnNewVertex0(Tick tick)
		{
			NewVertex?.Invoke(tick);

			if (vertexes.FirstIndexPermited > -1)
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
			levels[^1].BuildNewRansac(vertexes.VertexList, 0, typeSigma, percentile);
			NewVertex += levels[^1].OnNewVertex;
			levels[^1].NewRansacNeed += OnBuildAscHandler;
			levels[^1].RebuildRansacNeed += OnRebuildAscHandler;
			levels[^1].StopRansac += OnStopRansac;
			NewLevel?.Invoke(levels[^1]);
			NewRansac?.Invoke(levels[^1].GetRansacs()[^1], levels.Count - 1);
		}
		private void OnRebuildAscHandler(int level, Ransac ransac)
		{
			levels[level].RebuildRansac(vertexes.VertexList.GetRange(ransac.firstTickIndex, vertexes.VertexList.Count - ransac.firstTickIndex), ransac.firstTickIndex, typeSigma, percentile);
			RebuildRansac?.Invoke(levels[level].GetRansacs()[^1], level);
		}
		public void OnBuildAscHandler(int level, Ransac lastRansac)
		{
			if (level == 0)
			{
				if (lastRansac.EndIndexTick > vertexes.FirstIndexPermited)
					return;
				else
				{
					int startInd = vertexes.GetFirstIndexForNew(lastRansac);
					levels[level].BuildNewRansac(vertexes.VertexList.GetRange(startInd, vertexes.VertexList.Count - startInd), startInd, typeSigma, percentile);
					NewRansac?.Invoke(levels[level].GetRansacs()[^1], level);
				}
			}
			else
			{
				if (lastRansac.EndIndexTick > levels[level - 1].LastIndexPermited)
					return;
				else
				{
					int startInd;
					Ransac lastContained = levels[level - 1].GetLastRansacContained(lastRansac);

					if (lastRansac.Slope > 0)
						startInd = vertexes.GetIndexOfMaxTickInRansac(lastContained);
					else
						startInd = vertexes.GetIndexOfMinTickInRansac(lastContained);

					levels[level].BuildNewRansac(vertexes.VertexList.GetRange(startInd, vertexes.VertexList.Count - startInd), startInd, typeSigma, percentile);
					NewRansac?.Invoke(levels[level].GetRansacs()[^1], level);
				}
			}
		}
		public void OnStopRansac(int level, Ransac lastRansac)
		{
			StopRansac?.Invoke(lastRansac, level);
		}
	}
}
