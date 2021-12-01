using System;
using System.Collections.Generic;
using System.IO;

namespace RansacRealTime
{
	public class Vertexes
	{
		
		public List<Tick> VertexList { get; private set; } = new();
		public List<RansacsHystory> hystories = new();
		/// <summary>
		/// номер вершины, gjckt 
		/// </summary>
		public int FirstIndexPermited { get; private set; } = -1;


		public event VertexHandler NewVertex;

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
				LoadAllHystories(path);
		}


		private void FindLastIndexPermited()
		{
			if (VertexList.Count < 2)
            {
				FirstIndexPermited = -1;
				return;
			}

			int index = VertexList.Count;
			double lastSlope = VertexList[index - 1].PRICE - VertexList[index - 2].PRICE;

			do
			{
				index--;

				if ((VertexList[index].PRICE - VertexList[index - 1].PRICE) * lastSlope < 0)
				{
					FirstIndexPermited = index - 1;
					return;
				}
			} 
			while (index > 1);

			FirstIndexPermited = -1;
		}
		public int GetFirstIndexForNew(Ransac lastRansac)
		{
			int startInd = lastRansac.EndIndexTick - 1;

			if (VertexList[startInd - 1].PRICE > VertexList[startInd].PRICE)
				startInd -= 1;
			
			return startInd;
		}
		public int GetIndexOfMinTickInRansac(Ransac ransac)
		{
			int minInd = ransac.FirstIndexTick;

			for (int i = ransac.FirstIndexTick; i < ransac.EndIndexTick; i++)
				if (VertexList[i].PRICE <= VertexList[minInd].PRICE)
					minInd = i;			
			
			return minInd;
		}
		public int GetIndexOfMaxTickInRansac(Ransac ransac)
		{
			int maxInd = ransac.FirstIndexTick;

			for (int i = ransac.FirstIndexTick; i < ransac.EndIndexTick; i++)
				if (VertexList[i].PRICE >= VertexList[maxInd].PRICE)
					maxInd = i;
			
			return maxInd;
		}


		public void OnNewVertex(Tick tick)
		{
			VertexList.Add(tick);
			FindLastIndexPermited();

			foreach (RansacsHystory hystory in hystories)
				hystory.OnNewVertex(tick);
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
				VertexList.Add(new Tick(Convert.ToInt64(data[0]), Convert.ToInt32(data[1]), (double)Convert.ToDecimal(data[2])));
			}
		}

		public virtual void SaveStandart(string path)
		{
			if(!new DirectoryInfo(path).Exists)
			{
				Directory.CreateDirectory(path);
			}
			using StreamWriter writer = new(path + "/vertexes.csv");
			writer.WriteLine("localIndex; globalIndex; price");
			foreach (Tick vertex in VertexList)
			{
				writer.WriteLine(vertex.ID.ToString() + ';' + vertex.VERTEXINDEX.ToString() + ';' + vertex.PRICE.ToString() + ';');
			}
		}
		public void SaveStandart(string path, bool saveHystoriesToo)
		{
			SaveStandart(path);
			if(saveHystoriesToo)
				SaveAllHystories(path);
		}


		public void SaveAllHystories(string path)
		{
			foreach(RansacsHystory hystory in hystories)
			{
				hystory.SaveStandart(path);
			}
		}

		/// <summary>
		/// warning:deletes all current ransac hystories if there was
		/// </summary>
		/// <param name="path"></param>
		/// <returns>List of loaded hystories</returns>
		private List<RansacsHystory> LoadAllHystories(string path)
		{
			hystories.Clear();
			DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();

			foreach(DirectoryInfo hDir in dirs)
			{
				if (hDir.Name.Contains("Hystory"))
				{
					new RansacsHystory(this, hDir.FullName);
				}
			}

			return hystories;
		}
	}
}
