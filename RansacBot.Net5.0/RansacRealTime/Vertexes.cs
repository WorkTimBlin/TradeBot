using System;
using System.Collections.Generic;
using System.IO;

namespace RansacRealTime
{
	public class Vertexes
	{
		#region Свойства

		/// <summary>
		/// Список вершин MonkeyN.
		/// </summary>
        public List<Tick> VertexList { get; private set; }
		/// <summary>
		/// Список подписантов(ранзаков) на вершины.
		/// </summary>
		public List<RansacHystory> Hystories { get; set; }
		/// <summary>
		/// Индекс последнего тика, в который выполнилось условия разнонаправленных ранзаков.
		/// </summary>
		public int LastIndexPermited { get; private set; }

		#endregion

		public event VertexHandler NewVertex;

		public Vertexes()
		{
			VertexList = new();
			Hystories = new();
			LastIndexPermited = -1;
		}

		public Vertexes(string path) : base()
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


		public void FindLastIndexPermited()
		{
			if (VertexList.Count < 2)
            {
				LastIndexPermited = -1;
				return;
			}

			int index = VertexList.Count;
			double lastSlope = VertexList[index - 1].PRICE - VertexList[index - 2].PRICE;

			do
			{
				index--;

				if ((VertexList[index].PRICE - VertexList[index - 1].PRICE) * lastSlope < 0)
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

			foreach (RansacHystory hystory in Hystories)
				hystory.OnNewVertex(tick);
		}
		public void OnNewVertex(Tick tick, VertexType vertexType)
		{
			OnNewVertex(tick);
			NewVertex?.Invoke(tick, vertexType);
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
		private void LoadStandart(string path)
		{
			using StreamReader reader = new(path + "/vertexes.csv");
			reader.ReadLine();
			while (!reader.EndOfStream)
			{
				string[] data = reader.ReadLine().Split(';');
				VertexList.Add(new Tick(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), (float)Convert.ToDecimal(data[2])));
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
			foreach(RansacHystory hystory in Hystories)
			{
				hystory.SaveStandart(path);
			}
		}

		/// <summary>
		/// warning:deletes all current ransac hystories if there was
		/// </summary>
		/// <param name="path"></param>
		/// <returns>List of loaded hystories</returns>
		private List<RansacHystory> LoadAllHystories(string path)
		{
			Hystories.Clear();
			DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();

			foreach(DirectoryInfo hDir in dirs)
			{
				if (hDir.Name.Contains("Hystory"))
				{
					Hystories.Add(new(this, hDir.FullName));
				}
			}

			return Hystories;
		}
	}
}
