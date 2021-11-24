using System;
using System.Collections.Generic;
using System.IO;

namespace RansacRealTime
{
	public class Vertexes
	{
		public List<Tick> vertexes { get; private set; } = new();
		public List<RansacHystory> hystories = new();
		public int Count { get { return vertexes.Count; } }
		public int lastIndexPermited { get; private set; } = -1;

		public Vertexes()
		{

		}

		public Vertexes(string path)
		{
			LoadStandart(path);
			FindLastIndexPermited();
		}

		public Vertexes(string path, bool loadHystoryesToo)
		{
			LoadStandart(path);
			FindLastIndexPermited();
			if (loadHystoryesToo)
			{
				LoadAllHystories(path);
			}
		}

		public void FindLastIndexPermited()
		{
			int ind = vertexes.Count;
			if (ind < 2)
			{
				lastIndexPermited = -1;
				return;
			}
			float lastSlope = vertexes[ind - 1].PRICE - vertexes[ind - 2].PRICE;
			do
			{
				ind--;
				if ((vertexes[ind].PRICE - vertexes[ind - 1].PRICE) * lastSlope < 0)
				{
					lastIndexPermited = ind - 1;
					return;
				}
			} while (ind > 1);
			lastIndexPermited = -1;
		}

		public List<Tick> GetRange(int index, int count)
		{
			return vertexes.GetRange(index, count);
		}

		public int GetFirstIndexForNew(Ransac lastRansac)
		{
			int startInd = lastRansac.LastIndexTick - 1;
			if (vertexes[startInd - 1].PRICE > vertexes[startInd].PRICE)
			{
				startInd -= 1;
			}
			return startInd;
		}

		public int GetIndexOfMinTickInRansac(Ransac ransac)
		{
			int minInd = ransac.FirstIndexTick;
			for (int i = ransac.FirstIndexTick; i < ransac.LastIndexTick; i++)
			{
				if (vertexes[i].PRICE <= vertexes[minInd].PRICE)
				{
					minInd = i;
				}
			}
			return minInd;
		}

		public int GetIndexOfMaxTickInRansac(Ransac ransac)
		{
			int maxInd = ransac.FirstIndexTick;
			for (int i = ransac.FirstIndexTick; i < ransac.LastIndexTick; i++)
			{
				if (vertexes[i].PRICE >= vertexes[maxInd].PRICE)
				{
					maxInd = i;
				}
			}
			return maxInd;
		}


		public event VertexHandler NewVertex;

		public void OnNewVertex(Tick tick)
		{
			vertexes.Add(tick);
			FindLastIndexPermited();
			foreach (RansacHystory hystory in hystories)
			{
				hystory.OnNewVertex(tick);
			}
		}

		public void OnNewVertex(Tick tick, VertexType vertexType)
		{
			OnNewVertex(tick);
			NewVertex?.Invoke(tick, vertexType);
		}

		private void LoadStandart(string path)
		{
			using StreamReader reader = new(path + "/vertexes.csv");
			reader.ReadLine();
			while (!reader.EndOfStream)
			{
				string[] data = reader.ReadLine().Split(';');
				vertexes.Add(new Tick(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), (float)Convert.ToDecimal(data[2])));
			}
		}
		public void SaveStandart(string path)
		{
			using StreamWriter writer = new(path + "/vertexes.csv");
			writer.WriteLine("localIndex; globalIndex; price");
			foreach (Tick vertex in vertexes)
			{
				writer.WriteLine(vertex.ID.ToString() + ';' + vertex.VERTEXINDEX.ToString() + ';' + vertex.PRICE.ToString() + ';');
			}
		}

		/// <summary>
		/// warning:deletes all current ransac hystories if there was
		/// </summary>
		/// <param name="path"></param>
		/// <returns>List of loaded hystories</returns>
		private List<RansacHystory> LoadAllHystories(string path)
		{
			hystories.Clear();
			DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();
			foreach(DirectoryInfo hDir in dirs)
			{
				if (hDir.Name.Contains("Hystory"))
				{
					hystories.Add(new(this, hDir.FullName));
				}
			}
			return hystories;
		}
	}
}
