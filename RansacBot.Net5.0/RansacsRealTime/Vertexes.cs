using System;
using System.Collections.Generic;
using System.IO;

namespace RansacsRealTime
{
	public class Vertexes
	{
		
		public readonly List<Tick> vertexList = new();
		public readonly List<RansacsCascade> cascades = new();
		public int LastIndexPermited { get; private set; } = -1;

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
			if (loadHystoryesToo) LoadAllHystories(path);
		}


		private void FindLastIndexPermited()
		{
			if (vertexList.Count < 2)
            {
				LastIndexPermited = -1;
				return;
			}

			int index = vertexList.Count;
			double lastSlope = vertexList[index - 1].PRICE - vertexList[index - 2].PRICE;

			do
			{
				index--;

				if ((vertexList[index].PRICE - vertexList[index - 1].PRICE) * lastSlope < 0)
				{
					LastIndexPermited = index - 1;
					return;
				}
			} 
			while (index > 1);

			LastIndexPermited = -1;
		}
		public int GetFirstIndexForNew(Ransac lastRansac)
		{
			int startInd = lastRansac.EndTickIndex - 1;

			if (vertexList[startInd - 1].PRICE > vertexList[startInd].PRICE)
				startInd -= 1;
			
			return startInd;
		}
		public int GetIndexOfMinTickInRansac(Ransac ransac)
		{
			int minInd = ransac.firstTickIndex;

			for (int i = ransac.firstTickIndex; i < ransac.EndTickIndex; i++)
				if (vertexList[i].PRICE <= vertexList[minInd].PRICE)
					minInd = i;			
			
			return minInd;
		}
		public int GetIndexOfMaxTickInRansac(Ransac ransac)
		{
			int maxInd = ransac.firstTickIndex;

			for (int i = ransac.firstTickIndex; i < ransac.EndTickIndex; i++)
				if (vertexList[i].PRICE >= vertexList[maxInd].PRICE)
					maxInd = i;
			
			return maxInd;
		}

		public void OnNewVertex(Tick tick)
		{
			vertexList.Add(tick);
			FindLastIndexPermited();

			foreach (RansacsCascade hystory in cascades)
				hystory.OnNewVertex(tick);
		}
		public void OnNewVertex(Tick tick, VertexType vertexType)
		{
			OnNewVertex(tick);
			NewVertex?.Invoke(tick, vertexType);
		}

		private const string stdFileName = "vertexes.csv";
		private void LoadStandart(string path, string fileName = stdFileName)
		{
			using StreamReader reader = new(path + @"\" + fileName);
			reader.ReadLine();
			while (!reader.EndOfStream)
			{
				string[] data = reader.ReadLine().Split(';');
				vertexList.Add(new Tick(Convert.ToInt64(data[0]), Convert.ToInt32(data[1]), (double)Convert.ToDecimal(data[2])));
			}
		}
		public virtual void SaveStandart(string path, string fileName = stdFileName)
		{
			if(!new DirectoryInfo(path).Exists)
			{
				Directory.CreateDirectory(path);
			}
			using StreamWriter writer = new(path + @"\" + fileName);
			writer.WriteLine("localIndex; globalIndex; price");
			foreach (Tick vertex in vertexList)
			{
				writer.WriteLine(vertex.ToString());
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
			foreach(RansacsCascade hystory in cascades)
			{
				hystory.SaveStandart(path);
			}
		}
		/// <summary>
		/// warning:deletes all current ransac hystories if there was
		/// </summary>
		/// <param name="path"></param>
		/// <returns>List of loaded hystories</returns>
		private List<RansacsCascade> LoadAllHystories(string path)
		{
			cascades.Clear();
			DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();

			foreach(DirectoryInfo hDir in dirs)
			{
				if (hDir.Name.Contains("Hystory"))
				{
					new RansacsCascade(this, hDir.FullName);
				}
			}

			return cascades;
		}
	}
}
