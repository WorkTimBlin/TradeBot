using System;
using System.IO;

namespace RansacRealTime
{
    public delegate void VertexHandler(Tick tick, VertexType vertexType);

	public interface IVertexFinder
	{
		void OnNewTick(Tick tick);
		public event VertexHandler NewVertex;
	}

	public interface IVertexFilter
	{
		void OnNewVertex(Tick tick, VertexType vertexType);
		public event VertexHandler NewVertex;
	}

	public class MonkeyNFilter : IVertexFinder
	{
		public readonly double n;
		int count = 0;
		Tick max;
		Tick min;
		Tick last;
		Tick lastReturned;

		public MonkeyNFilter(double n)
		{
			this.n = n;
			OnNewTickChooser = OnNewTick0;
		}

		public MonkeyNFilter(double n, Tick last)
		{
			this.n = n;
			this.count = last.VERTEXINDEX + 1;
			this.lastReturned = last;
			this.max = last;
			this.min = last;
			this.last = last;
			OnNewTickChooser = OnNewTick1;
		}

		private const string stdFileName = "monkeyNFilter.csv";
		public MonkeyNFilter(string path, string name = stdFileName)
		{
			using(StreamReader reader = new(path + @"/" + name))
			{
				this.n = Convert.ToDouble(reader.ReadLine().Split(';')[1]);
				this.count = Convert.ToInt32(reader.ReadLine().Split(';')[1]);
				string line = reader.ReadLine();
				this.max = Tick.StandartParse(line.Substring(line.IndexOf(';')));
				line = reader.ReadLine();
				this.min = Tick.StandartParse(line.Substring(line.IndexOf(';')));
				line = reader.ReadLine();
				this.last = Tick.StandartParse(line.Substring(line.IndexOf(';')));
				line = reader.ReadLine();
				this.lastReturned = Tick.StandartParse(line.Substring(line.IndexOf(';') + 1));
			}
			if(lastReturned.Equals(max))
			{
				if(lastReturned.PRICE - last.PRICE == n) OnNewTickChooser = OnNewTickSearchHigh;
				else OnNewTickChooser = OnNewTickSearchLow;
			}
			else
			{
				if(last.PRICE - lastReturned.PRICE == n) OnNewTickChooser = OnNewTickSearchLow;
				else OnNewTickChooser = OnNewTickSearchHigh;
			}
		}

		public void SaveStandart(string path, string name = stdFileName)
		{
			using(StreamWriter writer = new(path + @"/" + name))
			{
				foreach(string line in SerializeForCSV())
				{
					writer.WriteLine(line);
				}
			}
		}

		public string[] SerializeForCSV()
		{
			string[] lines = new string[6];
			lines[0] = "N;" + this.n.ToString();
			lines[1] = "count;" + this.count.ToString();
			lines[2] = "max;" + this.max.ToString();
			lines[3] = "min;" + this.min.ToString();
			lines[4] = "last;" + this.last.ToString();
			lines[5] = "lastReturned;" + this.lastReturned.ToString();
			return lines;
		}

		public event VertexHandler NewVertex;

		public delegate void DelOnNewTick(Tick tick);

		private DelOnNewTick OnNewTickChooser;

		private void RaiseNewVertexEvent(Tick tick, VertexType vertexType)
		{
			if (tick.Equals(lastReturned))
			{
				return;
			}
			lastReturned = tick;
			tick = new Tick(tick.ID, count, tick.PRICE);
			count++;
			NewVertex?.Invoke(tick, vertexType);
		}

		public void OnNewTick(Tick tick)
		{
			OnNewTickChooser(tick);
		}

		private void OnNewTick0(Tick tick)
		{
			//Console.WriteLine("first tick!");
			max = tick;
			min = tick;
			this.OnNewTickChooser = OnNewTick1;
		}

		private void OnNewTick1(Tick tick)
		{
			if (tick.PRICE > max.PRICE)
			{
				max = tick;
			}
			if (tick.PRICE < min.PRICE)
			{
				min = tick;
			}
			if (tick.PRICE >= min.PRICE + n)
			{
				last = tick;
				OnNewTickChooser = OnNewTickSearchHigh;
				RaiseNewVertexEvent(min, VertexType.Low);
			}
			if (tick.PRICE <= max.PRICE - n)
			{
				last = tick;
				OnNewTickChooser = OnNewTickSearchLow;
				RaiseNewVertexEvent(max, VertexType.High);
			}
		}

		private void OnNewTickSearchHigh(Tick tick)
		{
			if (tick.PRICE > max.PRICE)
			{
				max = tick;
				if (tick.PRICE >= last.PRICE + n)
				{
					last = tick;
					RaiseNewVertexEvent(tick, VertexType.Monkey);
				}
			}
			if (tick.PRICE <= max.PRICE - n)
			{
				last = tick;
				min = tick;
				OnNewTickChooser = OnNewTickSearchLow;
				RaiseNewVertexEvent(max, VertexType.High);
			}
		}

		private void OnNewTickSearchLow(Tick tick)
		{
			if (tick.PRICE < min.PRICE)
			{
				min = tick;
				if (tick.PRICE <= last.PRICE - n)
				{
					last = tick;
					RaiseNewVertexEvent(tick, VertexType.Monkey);
				}
			}
			if (tick.PRICE >= min.PRICE + n)
			{
				last = tick;
				max = tick;
				OnNewTickChooser = OnNewTickSearchHigh;
				RaiseNewVertexEvent(min, VertexType.Low);
			}
		}

		public bool Equals(MonkeyNFilter other)
		{
			if (this.n == other.n &&
				this.max.Equals(other.max) &&
				this.min.Equals(other.min) &&
				this.last.Equals(other.last) &&
				this.lastReturned.Equals(other.lastReturned) &&
				this.OnNewTickChooser.Method.Equals(other.OnNewTickChooser.Method)) return true;
			return false;
		}

	}

}
