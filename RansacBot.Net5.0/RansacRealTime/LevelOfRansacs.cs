using System;
using System.Collections.Generic;

namespace RansacRealTime
{
	public class LevelOfRansacs
	{
		#region Свойства

		/// <summary>
		/// Уровень раназаков.
		/// </summary>
		private int Level { get; set; }
		/// <summary>
		/// Список ранзаков этого уровня.
		/// </summary>
		private List<Ransac> Ransacs { get; set; }
		/// <summary>
		/// True - Ранзак в данный момент строится. <br/>
		/// False - Ранзак не строится.
		/// </summary>
		public bool IsBuilding { get; private set; } = false;
		/// <summary>
		/// Индекс последнего тика, в который выполнилось условие построение ранзака.
		/// </summary>
		public int LastIndexPermited { get; private set; } = -1;

		#endregion


		public delegate void NewTickHandler(Tick tick);
		public event NewTickHandler NewVertex;

		public delegate void RansacHandler(int level, Ransac ransac);
		public event RansacHandler StopRansac;
		public event RansacHandler NewRansacNeed;
		public event RansacHandler RebuildRansacNeed;


		public LevelOfRansacs(int level)
		{
			Ransacs = new();
			Level = level;
		}
		public LevelOfRansacs(int level, string path)
		{
			Ransacs = new();
			Level = level;
			LoadStandart(path);
		}




		public void SetRansacs(List<Ransac> ransacs, bool building = false)
		{
			Ransacs = ransacs;
			IsBuilding = building;
		}
		public void SaveStandart(string path)
		{
			using System.IO.StreamWriter file = new(path + "/ransacLevel-" + Level.ToString() + ".csv");
			file.WriteLine("X1;X2;X3;X4;Slope;Intercept;Sigma;errorTreshold;" + IsBuilding.ToString());

			foreach (Ransac ransac in Ransacs)
			{
				string line = ransac.FirstIndexTick.ToString() + ';'
					+ ransac.FirstIndexBuild.ToString() + ';'
					+ ransac.LastIndexRebuild.ToString() + ';'
					+ (ransac.LastIndexTick - 1).ToString() + ';' +
					((decimal)ransac.Slope).ToString() + ';' +
					((decimal)ransac.Intercept).ToString() + ';' +
					((decimal)ransac.Sigma).ToString() + ';' +
					((decimal)ransac.ErrorTreshold).ToString();
				file.WriteLine(line);
			}
		}
		private void LoadStandart(string path)
		{
			using System.IO.StreamReader reader = new(path + "/ransacLevel-" + this.Level + ".csv");
			IsBuilding = Convert.ToBoolean(reader.ReadLine().Split(';')[^1]);
			Ransacs = new();

			while (!reader.EndOfStream)
			{
				string[] data = reader.ReadLine().Split(';');
				Ransacs.Add(new Ransac(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]),
					Convert.ToInt32(data[2]), Convert.ToInt32(data[3]) - Convert.ToInt32(data[0]) + 1,
					(float)Convert.ToDecimal(data[4]), (float)Convert.ToDecimal(data[5]),
					(float)Convert.ToDecimal(data[6]), (float)Convert.ToDecimal(data[7])));
			}

			if (IsBuilding)
				ConnectLastRansac();
			
			FindLastPermitedIndex();
		}

		public Ransac[] GetRansacsAsArray()
		{
			return Ransacs.ToArray();
		}
		public List<Ransac> GetRansacs()
		{
			return Ransacs;
		}
		public List<Ransac> GetRansacsFinishedOnly()
		{
			List<Ransac> ransacs = new();

			for (int i = 0; i < Ransacs.Count - 1; i++)
				ransacs.Add(Ransacs[i]);
			
			if (!IsBuilding)
				ransacs.Add(Ransacs[^1]);

			return ransacs;
		}

		/// <summary>
		/// returns last ransac that fully contained in given
		/// </summary>
		/// <param name="ind"></param>
		/// <returns></returns>
		public Ransac GetLastRansacContained(Ransac ransac)
		{
			int ran = Ransacs.Count;

			if (IsBuilding)
				ran--;
			
			do
			{
				ran--;

				if (Ransacs[ran].LastIndexTick <= ransac.LastIndexTick)
					return Ransacs[ran];
			}
			while (ran > 0);

			throw new Exception("no ransac stopped inside given ransac");
		}
		private void FindLastPermitedIndex()
		{
			int rans = Ransacs.Count;

			if (IsBuilding)
				rans--;
			
			double slopeLastFinished = Ransacs[rans - 1].Slope;

			do
			{
				rans--;

				if (Ransacs[rans].Slope * slopeLastFinished < 0)
				{
					LastIndexPermited = Ransacs[rans].FirstIndexTick;
					return;
				}
			}
			while (rans > 0);

			LastIndexPermited = -1;
			return;
		}
		/// <summary>
		/// checks, if there is two differently directed ransacs after given index
		/// </summary>
		/// <param name="lastUsedIndex"></param>
		/// <returns></returns>
		public bool IsTwoDifferentlyDirectedAfter(int lastUsedIndex)
		{
			int ransFromEnd = Ransacs.Count;

			do
			{
				ransFromEnd--;

				if (Ransacs[ransFromEnd].Slope * Ransacs[^1].Slope < 0)
					return Ransacs.Count < 1 || Ransacs[ransFromEnd].FirstIndexTick > lastUsedIndex;
			}
			while (ransFromEnd > 0);

			return false;
		}

		/// <summary>
		/// builds new ransac if no is currently building
		/// </summary>
		/// <param name="ticks"></param>
		private void CreateNewRansac(List<Tick> ticks, int firstIndex, TypeSigma typeSigma, double percentile = 0)
		{
			if (IsBuilding)
				throw new Exception("can't create ransac while previous is not finished");
			
			IsBuilding = true;
			Ransacs.Add(new Ransac(ticks, firstIndex, TypeSigma.Sigma, percentile));
			ConnectLastRansac();
		}
		private void ConnectLastRansac()
		{
			NewVertex += Ransacs[^1].OnNewVertex;
			Ransacs[^1].StopRansac += OnRansacStop;
			Ransacs[^1].NeedRebuilding += OnRebuildRansacNeed;
		}
		public void RebuildRansac(List<Tick> ticks, int firstIndex, TypeSigma typeSigma, double percentile = 0)
		{
			Ransacs[^1].Rebuild(new Ransac(ticks, firstIndex, typeSigma, percentile));
		}
		public void OnRebuildRansacNeed(Ransac ransac)
		{
			RebuildRansacNeed?.Invoke(this.Level, Ransacs[^1]);
		}
		public void BuildNewRansac(List<Tick> ticks, int firstIndex, TypeSigma typeSigma, double percentile = 0)
		{
			CreateNewRansac(ticks, firstIndex, typeSigma, percentile);
		}
		public void OnNewVertex(Tick tick)
		{
			if (!IsBuilding)
				NewRansacNeed?.Invoke(Level, Ransacs[^1]);
			else
				NewVertex?.Invoke(tick);
		}
		public void OnRansacStop(Ransac ransac)
		{
			IsBuilding = false;
			NewVertex -= ransac.OnNewVertex;
			ransac.StopRansac -= OnRansacStop;
			FindLastPermitedIndex();
			StopRansac?.Invoke(Level, ransac);
		}

	}
}
