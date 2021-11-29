using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RansacRealTime
{
    /// <summary>
    /// Класс работает с ранзаками.
    /// </summary>
    public class RansacHystory
	{
		#region Свойства

		/// <summary>
		/// Список всех уровней на данный момент.
		/// </summary>
		public List<LevelOfRansacs> Levels { get; private set; } = new();
		/// <summary>
		/// Ссылка на вершины MonkeyN.
		/// </summary>
		public Vertexes Vertexes { get; private set; } = new();
		/// <summary>
		/// Тип сигмы текущих ранзаков (Enum).
		/// </summary>
		public TypeSigma Type { get; private set; }
		/// <summary>
		/// Номер (0 - 100) персентиля для типа сигмы - доверительный интервал. <br/>
		/// В остальных случаях не используется.
		/// </summary>
		public double Percentile { get; private set; }
		/// <summary>
		/// Максимально допустимый для поиска уровень. (По умолчанию 7). (Максимальный уровень сейчас - это индекс в списке уровней, причем списка без первого уровня. Поправить это потом).
		/// </summary>
		public int MaxLevel { get; private set; } = 5;
		/// <summary>
		/// True - достигнут максимальный уровень ранзаков.
		/// </summary>
		public bool IsReachedMaxLevel { get { return Levels.Count == MaxLevel; } }

		#endregion

		#region Конструкторы

		public RansacHystory(Vertexes vertexes, TypeSigma typeSigma, double percentile = 90)
		{
			SetVertexes(vertexes);
			Type = typeSigma;
			Percentile = percentile;
			OnNewVertexChooser = OnNewVertex0;
		}
		public RansacHystory(Vertexes vertexes, TypeSigma typeSigma, int maxLevels, double percentile = 90)
		{
			SetVertexes(vertexes);
			Type = typeSigma;
			MaxLevel = maxLevels;
			OnNewVertexChooser = OnNewVertex0;
			Percentile = percentile;
		}
		public RansacHystory(Vertexes vertexes, TypeSigma typeSigma, string path)
		{
			SetVertexes(vertexes);
			OnNewVertexChooser = OnNewVertex1;
			path += @"\Hystory" + typeSigma.ToString();
			LoadMetadata(path);
			LoadLevelsStandart(path);
		}
		public RansacHystory(Vertexes vertexes, string path)
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
			return Levels.Select(x => x.GetRansacsAsArray()).ToArray();
		}


		public void SaveStandart(string path)
		{
			// добавить комментарий: Warning: deletes the entire directory to save recursevly before writing!
			path = path + @"\Hystory" + Type.ToString();

			if (Directory.Exists(path))
				Directory.Delete(path, true);

			Directory.CreateDirectory(path);
			SaveLevels(path);
			SaveMetadata(path);
		}
		private void SaveMetadata(string path)
		{
			using StreamWriter writer = new(path + "/metadata.csv");
			writer.WriteLine("typeSigma;" + Type.ToString());
			writer.WriteLine("percentile;" + Percentile.ToString());
		}
		private void SaveLevels(string path)
		{
			foreach (LevelOfRansacs level in Levels)
				level.SaveStandart(path);
		}
		private void LoadMetadata(string path)
		{
			using StreamReader reader = new(path + "/metadata.csv");
			string line = reader.ReadLine().Split(';')[1];
			Type = (TypeSigma)Enum.Parse(typeof(TypeSigma), line);
			Percentile = Convert.ToDouble(reader.ReadLine().Split(';')[1]);
		}
		private void LoadLevelsStandart(string path)
		{
			FileInfo[] files = new DirectoryInfo(path).GetFiles();
			int numberOfLevels = files.Count(x => x.Name.StartsWith("ransacLevel-"));

			for (int i = 0; i < numberOfLevels; i++)
			{
				Levels.Add(new(i, path));
				NewVertex += Levels[^1].OnNewVertex;
				Levels[^1].NewRansacNeed += OnBuildAscHandler;
				Levels[^1].RebuildRansacNeed += OnRebuildAscHandler;
			}
		}


		#region Обработчики

		private void SetVertexes(Vertexes vertexes)
		{
			Vertexes = vertexes;
			vertexes.Hystories.Add(this);
		}
		public void OnNewVertex(Tick tick)
		{
			OnNewVertexChooser(tick);
		}
		private void OnNewVertex0(Tick tick)
		{
			NewVertex?.Invoke(tick);

			if (Vertexes.LastIndexPermited > -1)
			{
				AddNewLevel();
				OnNewVertexChooser = OnNewVertex1;
			}
		}
		private void OnNewVertex1(Tick tick)
		{
			NewVertex?.Invoke(tick);

			if (Levels.Count < MaxLevel && Levels[^1].LastIndexPermited >= 0)
				AddNewLevel();
		}
		private void AddNewLevel()
		{
			Levels.Add(new LevelOfRansacs(Levels.Count));
			Levels[^1].BuildNewRansac(Vertexes.VertexList, 0, Type, Percentile);
			NewVertex += Levels[^1].OnNewVertex;
			Levels[^1].NewRansacNeed += OnBuildAscHandler;
			Levels[^1].RebuildRansacNeed += OnRebuildAscHandler;
			Levels[^1].StopRansac += OnStopRansac;
			NewLevel?.Invoke(Levels[^1]);
			NewRansac?.Invoke(Levels[^1].GetRansacs()[^1], Levels.Count - 1);
		}
		private void OnRebuildAscHandler(int level, Ransac ransac)
		{
			Levels[level].RebuildRansac(Vertexes.VertexList.GetRange(ransac.FirstIndexTick, Vertexes.VertexList.Count - ransac.FirstIndexTick), ransac.FirstIndexTick, Type, Percentile);
			RebuildRansac?.Invoke(Levels[level].GetRansacs()[^1], level);
		}
		public void OnBuildAscHandler(int level, Ransac lastRansac)
		{
			if (level == 0)
			{
				if (lastRansac.EndIndexTick > Vertexes.LastIndexPermited)
					return;
				else
				{
					int startInd = Vertexes.GetFirstIndexForNew(lastRansac);
					Levels[level].BuildNewRansac(Vertexes.VertexList.GetRange(startInd, Vertexes.VertexList.Count - startInd), startInd, Type, Percentile);
					NewRansac?.Invoke(Levels[level].GetRansacs()[^1], level);
				}
			}
			else
			{
				if (lastRansac.EndIndexTick > Levels[level - 1].LastIndexPermited)
					return;
				else
				{
					int startInd;
					Ransac lastContained = Levels[level - 1].GetLastRansacContained(lastRansac);

					if (lastRansac.Slope > 0)
						startInd = Vertexes.GetIndexOfMaxTickInRansac(lastContained);
					else
						startInd = Vertexes.GetIndexOfMinTickInRansac(lastContained);

					Levels[level].BuildNewRansac(Vertexes.VertexList.GetRange(startInd, Vertexes.VertexList.Count - startInd), startInd, Type, Percentile);
					NewRansac?.Invoke(Levels[level].GetRansacs()[^1], level);
				}
			}
		}
		public void OnStopRansac(int level, Ransac lastRansac)
		{
			StopRansac?.Invoke(lastRansac, level);
		}

		#endregion
	}
}
