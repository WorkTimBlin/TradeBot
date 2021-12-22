using System;
using System.IO;
using System.Collections;


namespace RansacsRealTime
{
    public delegate void VertexHandler(Tick tick, VertexType vertexType);
	public delegate void ExtremumHandler(Tick extremum, VertexType vertexType, Tick current);

	public interface IVertexFinder
	{
		void OnNewTick(Tick tick);
		public event VertexHandler NewVertex; 
		public event VertexHandler LastVertexWasExtremum;
	}

	public interface IVertexFilter
	{
		void OnNewVertex(Tick tick, VertexType vertexType);
		public event VertexHandler NewVertex;
	}

	public interface IExtremumFilter
	{
		void OnNewExtremum(Tick extremum, VertexType vertexType, Tick current);
		public event ExtremumHandler NewExtremum;
	}

	public class MonkeyNFinder : IVertexFinder
	{
		public readonly double n;
		int count = 0;
		Tick max;
		Tick min;
		Tick last;
		Tick lastReturned;

		public MonkeyNFinder(double n)
		{
			this.n = n;
			OnNewTickChooser = OnNewTick0;
		}
		private const string stdFileName = "monkeyNFilter.csv";
		public MonkeyNFinder(string path, string name = stdFileName)
		{
			var data = LoadStandart(path, name);
			this.n = data.n;
			this.count = data.count;
			this.max = data.max;
			this.min = data.min;
			this.last = data.last;
			this.lastReturned = data.lastReturned;
			OnNewTickChooser = data.onNewTickChooser;
		}

		public 
		(double n,
		int count,
		Tick max,
		Tick min,
		Tick last,
		Tick lastReturned,
		TickHandler onNewTickChooser)
		LoadStandart(string path, string name = stdFileName)
		{
			using StreamReader reader = new(path + @"/" + name);
			double n = Convert.ToDouble(reader.ReadLine().Split(';')[1]);
			int count = Convert.ToInt32(reader.ReadLine().Split(';')[1]);
			string line = reader.ReadLine();
			Tick max = Tick.StandartParse(line.Substring(line.IndexOf(';')));
			line = reader.ReadLine();
			Tick min = Tick.StandartParse(line.Substring(line.IndexOf(';')));
			line = reader.ReadLine();
			Tick last = Tick.StandartParse(line.Substring(line.IndexOf(';')));
			line = reader.ReadLine();
			Tick lastReturned = Tick.StandartParse(line.Substring(line.IndexOf(';') + 1));
			line = reader.ReadLine();
			TickHandler onNewTickChooser = GetOnNewTickChooserFromString(line.Substring(line.IndexOf(';') + 1));
			return (n, count, max, min, last, lastReturned, onNewTickChooser);
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
			string[] lines = new string[7];
			lines[0] = "N;" + this.n.ToString();
			lines[1] = "count;" + this.count.ToString();
			lines[2] = "max;" + this.max.ToString();
			lines[3] = "min;" + this.min.ToString();
			lines[4] = "last;" + this.last.ToString();
			lines[5] = "lastReturned;" + this.lastReturned.ToString();
			lines[6] = "SearchingFor;" + GetWhatNowSearchingFor();
			return lines;
		}
		private string GetWhatNowSearchingFor()
		{
			if (OnNewTickChooser == OnNewTick0) return "tick";
			else if (OnNewTickChooser == OnNewTick1) return "vertex";
			else if (OnNewTickChooser == OnNewTickSearchHigh) return "high";
			else if (OnNewTickChooser == OnNewTickSearchLow) return "low";
			else throw new Exception("on new tick chooser not set to correct function");
		}
		private TickHandler GetOnNewTickChooserFromString(string whatSearchingFor)
		{
			if (whatSearchingFor == "tick") return OnNewTick0;
			else if (whatSearchingFor == "vertex") return OnNewTick1;
			else if (whatSearchingFor == "high") return OnNewTickSearchHigh;
			else if (whatSearchingFor == "low") return OnNewTickSearchLow;
			else throw new ArgumentException("Input string was not in correct format");
		}
		public void ReturnToLastReturned()
		{
			this.max = this.lastReturned;
			this.min = this.lastReturned;
		}

		public event VertexHandler NewVertex;
		public event VertexHandler LastVertexWasExtremum;
		public event ExtremumHandler NewExtremum;

		private void RaiseNewVertexEvent(Tick tick, VertexType vertexType)
		{
			if (tick.Equals(lastReturned))
			{
				tick = new Tick(tick.ID, count, tick.PRICE);
				LastVertexWasExtremum?.Invoke(tick, vertexType);
				return;
			}
			lastReturned = tick;
			tick = new Tick(tick.ID, count, tick.PRICE);
			count++;
			NewVertex?.Invoke(tick, vertexType);
		}
		private void RaiseNewExtremumEvent(Tick extremum, VertexType vertexType, Tick current)
		{
			extremum = new Tick(extremum.ID, count, extremum.PRICE);
			NewExtremum?.Invoke(extremum, vertexType, current);
		}

		private TickHandler OnNewTickChooser;
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
				RaiseNewExtremumEvent(min, VertexType.Low, tick);
			}
			if (tick.PRICE <= max.PRICE - n)
			{
				last = tick;
				OnNewTickChooser = OnNewTickSearchLow;
				RaiseNewVertexEvent(max, VertexType.High);
				RaiseNewExtremumEvent(max, VertexType.High, tick);
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
				RaiseNewExtremumEvent(max, VertexType.High, tick);
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
				RaiseNewExtremumEvent(min, VertexType.Low, tick);
			}
		}
		public bool Equals(MonkeyNFinder other)
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
