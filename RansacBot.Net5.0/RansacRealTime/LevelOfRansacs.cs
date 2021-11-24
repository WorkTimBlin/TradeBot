using System;
using System.Collections.Generic;

namespace RansacRealTime
{
	public class LevelOfRansacs
	{
		int level;
		List<Ransac> ransacs = new();
		public bool buildingRansac { get; private set; } = false;
		public int lastIndexPermited { get; private set; } = -1;

		public delegate void NewTickHandler(Tick tick);

		public event NewTickHandler NewVertex;

		public delegate void RansacHandler(int level, Ransac ransac);

		public event RansacHandler StopRansac;
		public event RansacHandler NewRansacNeed;
		public event RansacHandler RebuildRansacNeed;

		public LevelOfRansacs(int level)
		{
			this.level = level;
		}

		public LevelOfRansacs(int level, string path)
		{
			this.level = level;
			this.LoadStandart(path);
		}

		public void SetRansacs(List<Ransac> ransacs, bool building = false)
		{
			this.ransacs = ransacs;
			this.buildingRansac = building;
		}

		public void SaveStandart(string path)
		{
			using System.IO.StreamWriter file = new(path + "/ransacLevel-" + this.level.ToString() + ".csv");
			file.WriteLine("X1;X2;X3;X4;Slope;Intercept;Sigma;errorTreshold;" + buildingRansac.ToString());
			foreach (Ransac ransac in ransacs)
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
			using System.IO.StreamReader reader = new(path + "/ransacLevel-" + this.level + ".csv");
			this.buildingRansac = Convert.ToBoolean(reader.ReadLine().Split(';')[^1]);
			ransacs = new();
			while (!reader.EndOfStream)
			{
				string[] data = reader.ReadLine().Split(';');
				ransacs.Add(new Ransac(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]),
					Convert.ToInt32(data[2]), Convert.ToInt32(data[3]) - Convert.ToInt32(data[0]) + 1,
					(float)Convert.ToDecimal(data[4]), (float)Convert.ToDecimal(data[5]),
					(float)Convert.ToDecimal(data[6]), (float)Convert.ToDecimal(data[7])));
			}
			if (buildingRansac)
			{
				ConnectLastRansac();
			}
			this.FindLastPermitedIndex();
		}

		public Ransac[] GetRansacsAsArray()
		{
			return this.ransacs.ToArray();
		}

		public List<Ransac> GetRansacs()
		{
			return this.ransacs;
		}

		public List<Ransac> GetRansacsFinishedOnly()
		{
			List<Ransac> ransacs = new();
			for (int i = 0; i < this.ransacs.Count - 1; i++)
			{
				ransacs.Add(this.ransacs[i]);
			}
			if (!buildingRansac)
			{
				ransacs.Add(this.ransacs[^1]);
			}
			return ransacs;
		}

		/// <summary>
		/// returns last ransac that fully contained in given
		/// </summary>
		/// <param name="ind"></param>
		/// <returns></returns>
		public Ransac GetLastRansacContained(Ransac ransac)
		{
			int ran = ransacs.Count;
			if (buildingRansac)
			{
				ran--;
			}
			do
			{
				ran--;
				if (ransacs[ran].LastIndexTick <= ransac.LastIndexTick)
				{
					return ransacs[ran];
				}
			} while (ran > 0);
			throw new Exception("no ransac stopped inside given ransac");
		}

		private void FindLastPermitedIndex()
		{
			int rans = ransacs.Count;
			if (buildingRansac)
			{
				rans--;
			}
			double slopeLastFinished = ransacs[rans - 1].Slope;
			do
			{
				rans--;
				if (ransacs[rans].Slope * slopeLastFinished < 0)
				{
					lastIndexPermited = ransacs[rans].FirstIndexTick;
					return;
				}
			}
			while (rans > 0);
			lastIndexPermited = -1;
			return;
		}

		/// <summary>
		/// checks, if there is two differently directed ransacs after given index
		/// </summary>
		/// <param name="lastUsedIndex"></param>
		/// <returns></returns>
		public bool IsTwoDifferentlyDirectedAfter(int lastUsedIndex)
		{
			int ransFromEnd = ransacs.Count;
			do
			{
				ransFromEnd--;
				if (ransacs[ransFromEnd].Slope * ransacs[^1].Slope < 0)
				{
					if (ransacs.Count < 1 ||
						ransacs[ransFromEnd].FirstIndexTick > lastUsedIndex)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
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
			if (buildingRansac)
			{
				throw new Exception("can't create ransac while previous is not finished");
			}
			buildingRansac = true;
			ransacs.Add(new Ransac(ticks, firstIndex, TypeSigma.Sigma, percentile));
			ConnectLastRansac();
		}

		private void ConnectLastRansac()
		{
			NewVertex += ransacs[^1].OnNewVertex;
			ransacs[^1].StopRansac += OnRansacStop;
			ransacs[^1].NeedRebuilding += OnRebuildRansacNeed;
		}

		public void RebuildRansac(List<Tick> ticks, int firstIndex, TypeSigma typeSigma, double percentile = 0)
		{
			ransacs[^1].Rebuild(new Ransac(ticks, firstIndex, typeSigma, percentile));
		}

		public void OnRebuildRansacNeed(Ransac ransac)
		{
			//OnRansacStop(ransac);
			//ransacs.RemoveAt(ransacs.Count - 1); FIXME
			RebuildRansacNeed?.Invoke(this.level, ransacs[^1]);
		}

		public void BuildNewRansac(List<Tick> ticks, int firstIndex, TypeSigma typeSigma, double percentile = 0)
		{
			//Console.WriteLine("building new ransac!");
			CreateNewRansac(ticks, firstIndex, typeSigma, percentile);
		}

		public void OnNewVertex(Tick tick)
		{
			if (!buildingRansac)
			{
				NewRansacNeed?.Invoke(this.level, this.ransacs[^1]);
			}
			else
			{
				NewVertex?.Invoke(tick);
			}
		}

		public void OnRansacStop(Ransac ransac)
		{
			buildingRansac = false;
			NewVertex -= ransac.OnNewVertex;
			ransac.StopRansac -= OnRansacStop;
			FindLastPermitedIndex();
			StopRansac?.Invoke(level, ransac);
		}

	}
}
