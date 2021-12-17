using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacsRealTime
{
	/// <summary>
	/// хранит объект Vertexes и манки-фильтр, на который подписан vertexes.
	/// </summary>
	public class RansacsSession
	{
		public readonly Vertexes vertexes;
		public readonly MonkeyNFilter monkeyNFilter;

		/// <summary>
		/// принимает объекты вершин и MonkeyNFilter
		/// </summary>
		/// <param name="vertexes"></param>
		/// <param name="monkeyNFilter"></param>
		public RansacsSession(Vertexes vertexes, MonkeyNFilter monkeyNFilter)
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

		private const string stdDirName = "RansacsSession";
		/// <summary>
		/// loads current session
		/// </summary>
		/// <param name="path"></param>
		/// <param name="loadCascades"></param>
		public RansacsSession(string path, string dirName = stdDirName, bool loadCascades = true)
		{
			path += @"\" + dirName;
			vertexes = new(path, loadCascades);
			monkeyNFilter = new(path);
			monkeyNFilter.NewVertex += this.vertexes.OnNewVertex;
		}

		public void OnNewTick(Tick tick)
		{
			this.monkeyNFilter.OnNewTick(tick);
		}

		public void SaveStandart(string path, string dirName = stdDirName)
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
