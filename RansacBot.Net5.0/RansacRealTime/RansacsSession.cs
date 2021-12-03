using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacRealTime
{
	/// <summary>
	/// хранит объект Vertexes и манки-фильтр, на который подписан vertexes.
	/// </summary>
	class RansacsSession
	{
		public readonly Vertexes vertexes;
		public readonly MonkeyNFilter monkeyNFilter;

		/// <summary>
		/// принимает объекты вершин и MonkeyNFilter
		/// </summary>
		/// <param name="vertexes"></param>
		/// <param name="monkeyNFilter"></param>
		public RansacsSession(in Vertexes vertexes, in MonkeyNFilter monkeyNFilter)
		{
			this.vertexes = vertexes;
			this.monkeyNFilter = monkeyNFilter;
			this.monkeyNFilter.NewVertex -= this.vertexes.OnNewVertex; //убеждаемся, что не подключим лишний раз
			this.monkeyNFilter.NewVertex += this.vertexes.OnNewVertex;
		}

		public RansacsSession(int N)
		{
			this.vertexes = new();
			this.monkeyNFilter = new(N);
			this.monkeyNFilter.NewVertex += this.vertexes.OnNewVertex;
		}
		/// <summary>
		/// loads current session
		/// </summary>
		/// <param name="path"></param>
		/// <param name="loadHystories"></param>
		public RansacsSession(string path, bool loadHystories)
		{
			vertexes = new(path, loadHystories);
			double N;
			using(StreamReader reader = new(path + @"/metadata"))
			{
				N = Convert.ToDouble(reader.ReadLine());
			}
			monkeyNFilter = new(N, vertexes.VertexList[^1]);
		}

		public void OnNewTick(Tick tick)
		{
			this.monkeyNFilter.OnNewTick(tick);
		}

		public void SaveStandart(string path, string dirName = "RansacsSession")
		{
			path += @"/" + dirName;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			vertexes.SaveStandart(path, true);
			monkeyNFilter.SaveStandart(path);
		}
	}
}
