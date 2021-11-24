using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RansacRealTime
{
	public class RansacHystory
	{
		int maxLevels = 100;
		List<LevelOfRansacs> levels = new();	//ransacs of levels higher than 1
		public Vertexes vertexes { get; private set; } = new();
		public TypeSigma typeSigma;
		public double percentile;

		#region Constructors

		public RansacHystory(Vertexes vertexes, TypeSigma typeSigma, double percentile = 90)
		{
			SetVertexes(vertexes);
			this.typeSigma = typeSigma;
			OnNewVertexChooser = OnNewVertex0;
			this.percentile = percentile;
			Console.WriteLine("percetnile set to " + this.percentile.ToString());
		}

		public RansacHystory(Vertexes vertexes, TypeSigma typeSigma, int maxLevels, double percentile = 90)
		{
			SetVertexes(vertexes);
			this.typeSigma = typeSigma;
			this.maxLevels = maxLevels;
			OnNewVertexChooser = OnNewVertex0;
			this.percentile = percentile;
		}

		public RansacHystory(Vertexes vertexes, string path)
		{
			SetVertexes(vertexes);
			OnNewVertexChooser = OnNewVertex1;
			LoadMetadata(path);
			LoadLevelsStandart(path);
		}

		public RansacHystory(Vertexes vertexes, TypeSigma typeSigma, string path)
		{
			SetVertexes(vertexes);
			OnNewVertexChooser = OnNewVertex1;
			path += @"\Hystory" + typeSigma.ToString();
			LoadMetadata(path);
			LoadLevelsStandart(path);
		}

		#endregion Constructors

		private void SetVertexes(Vertexes vertexes)
		{
			this.vertexes = vertexes;
			vertexes.hystories.Add(this);
		}

		private void LoadMetadata(string path)
		{
			using StreamReader reader = new(path + "/metadata.csv");
			string line = reader.ReadLine().Split(';')[1];
			this.typeSigma = (TypeSigma)Enum.Parse(typeof(TypeSigma), line);
			this.percentile = Convert.ToDouble(reader.ReadLine().Split(';')[1]);
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


		/// <summary>
		/// Warning: deletes the entire directory to save recursevly before writing!
		/// </summary>
		/// <param name="path"></param>
		public void SaveStandart(string path)
		{
			path = path + @"\Hystory" + typeSigma.ToString();
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}
			Directory.CreateDirectory(path);
			SaveLevels(path);
			SaveMetadata(path);
		}

		private void SaveMetadata(string path)
		{
			using StreamWriter writer = new(path + "/metadata.csv");
			writer.WriteLine("typeSigma;" + this.typeSigma.ToString());
			writer.WriteLine("percentile;" + this.percentile.ToString());
		}

		private void SaveLevels(string path)
		{
			foreach (LevelOfRansacs level in levels)
			{
				level.SaveStandart(path);
			}
		}

		public Ransac[][] GetAllLevelsAsArrays()
		{
			Ransac[][] levelsArr = new Ransac[levels.Count][];
			for (int i = 0; i < levels.Count; i++)
			{
				levelsArr[i] = levels[i].GetRansacsAsArray();
			}
			return levelsArr;
		}

		public LevelOfRansacs GetLevel(int level)
		{
			if (level >= levels.Count)
			{
				return new(level);
			}
			return levels[level];
		}

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

		public void OnNewVertex(Tick tick)
		{
			//Console.WriteLine(tick.USERINDEX.ToString() + '\t' + tick.PRICE.ToString());
			OnNewVertexChooser(tick);
		}

		private void OnNewVertex0(Tick tick)
		{
			NewVertex?.Invoke(tick);
			if (vertexes.lastIndexPermited > -1)
			{
				//Console.WriteLine("building first level of ransacs!");
				AddNewLevel();
				OnNewVertexChooser = OnNewVertex1;
			}
		}

		private void OnNewVertex1(Tick tick)
		{
			NewVertex?.Invoke(tick);
			if (levels.Count < maxLevels && levels[^1].lastIndexPermited >= 0)
			{
				AddNewLevel();
			}
		}

		private void AddNewLevel()
		{
			levels.Add(new LevelOfRansacs(levels.Count));
			levels[^1].BuildNewRansac(vertexes.vertexes, 0, typeSigma, percentile);
			NewVertex += levels[^1].OnNewVertex;
			levels[^1].NewRansacNeed += OnBuildAscHandler;
			levels[^1].RebuildRansacNeed += OnRebuildAscHandler;
			levels[^1].StopRansac += OnStopRansac;
			NewLevel?.Invoke(levels[^1]);
		}

		private void OnRebuildAscHandler(int level, Ransac ransac)
		{
			levels[level].RebuildRansac(vertexes.vertexes.GetRange(ransac.FirstIndexTick, vertexes.vertexes.Count - ransac.FirstIndexTick), ransac.FirstIndexTick, typeSigma, percentile);
			RebuildRansac?.Invoke(levels[level].GetRansacs()[^1], level);
		}

		public void OnBuildAscHandler(int level, Ransac lastRansac)
		{
			if (level == 0)
			{
				if (lastRansac.LastIndexTick > vertexes.lastIndexPermited)
				{
					return;
				}
				else
				{
					int startInd = vertexes.GetFirstIndexForNew(lastRansac);
					levels[level].BuildNewRansac(vertexes.vertexes.GetRange(startInd, vertexes.vertexes.Count - startInd), startInd, typeSigma, percentile);
					NewRansac?.Invoke(levels[level].GetRansacs()[^1], level);
				}
			}
			else
			{
				if (lastRansac.LastIndexTick > levels[level - 1].lastIndexPermited)
				{
					return;
				}
				else
				{
					int startInd;
					Ransac lastContained = levels[level - 1].GetLastRansacContained(lastRansac);
					if (lastRansac.Slope > 0)
					{
						startInd = vertexes.GetIndexOfMaxTickInRansac(lastContained);
					}
					else
					{
						startInd = vertexes.GetIndexOfMinTickInRansac(lastContained);
					}
					levels[level].BuildNewRansac(vertexes.GetRange(startInd, vertexes.Count - startInd), startInd, typeSigma, percentile);
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
