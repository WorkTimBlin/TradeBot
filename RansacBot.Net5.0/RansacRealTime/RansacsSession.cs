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
		public Vertexes vertexes;
		public MonkeyNFilter monkeyNFilter;

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
	}
}
