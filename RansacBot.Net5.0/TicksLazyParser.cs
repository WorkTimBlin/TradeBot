using FinamDataLoader;
using RansacsRealTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RansacBot
{
	public readonly struct TickWithDateTime
	{
		public readonly Tick tick;
		public readonly DateTime dateTime;

		public TickWithDateTime(Tick tick, DateTime dateTime)
		{
			this.tick = tick;
			this.dateTime = dateTime;
		}
	}
	public class TicksLazyParser : IEnumerable<Tick>, IList<Tick>, ITicksHystoryGetter
	{
		private readonly IList<string> rawStrings;
		public TicksLazyParser(IList<string> rawStrings)
		{
			this.rawStrings = rawStrings;
		}

		#region IEnumerable
		public IEnumerator<Tick> GetEnumerator()
		{
			foreach (string line in rawStrings)
			{
				yield return ParseTick(line);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (string line in rawStrings)
			{
				yield return ParseTick(line);
			}
		}
		#endregion
		#region IList
		public Tick this[int index] 
		{ 
			get { return ParseTick(rawStrings[index]); } 
			set { throw new Exception("cannot modify lazy parser!"); } 
		}
		public void Add(Tick tick)
		{
			throw new Exception("cannot modify lazy parser!");
		}
		public void Clear()
		{
			rawStrings.Clear();
		}
		public bool Contains(Tick tick)
		{
			foreach(Tick tick1 in this)
			{
				if (tick.Equals(tick1)) return true;
			}
			return false;
		}
		public void CopyTo(Tick[] targetArray, int startIndex)
		{
			foreach(Tick tick in this)
			{
				targetArray[startIndex] = tick;
				startIndex++;
			}
		}
		public int IndexOf(Tick tick)
		{
			for(int i = 0; i < rawStrings.Count; i++)
			{
				if (tick.Equals(this[i])) return i;
			}
			return -1;
		} 
		public void Insert(int index, Tick tick)
		{
			throw new Exception("cannot modify lazy parser!");
		}
		public void RemoveAt(int index)
		{
			rawStrings.RemoveAt(index);
		}
		public bool Remove(Tick tick)
		{
			int index = IndexOf(tick);
			if (index == -1) return false;
			rawStrings.RemoveAt(index);
			return true;
		}
		public int Count { get => rawStrings.Count; }
		public bool IsReadOnly { get => true; }
		#endregion
		#region ITicksHystoryGetter
		/// <summary>
		/// Warning: just returns this, from and to doesn't matter
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public IList<Tick> GetTicks(DateTime from, DateTime to)
		{
			return this;
		}
		#endregion
		static public Tick ParseTick(string[] data)
		{
			return new Tick(
				Convert.ToInt64(data[4]),
				0,
				(double)Convert.ToDouble(data[2], System.Globalization.CultureInfo.InvariantCulture));
		}
		static public Tick ParseTick(string line)
		{
			return ParseTick(line.Split(';', StringSplitOptions.RemoveEmptyEntries));
		}
	}

	public class TicksLazySequentialParser: IEnumerable<Tick>
	{
		private ITicksParser ticksParser;
		private readonly IEnumerable<string> rawStrings;
		public TicksLazySequentialParser(IEnumerable<string> rawStrings) : this(rawStrings, TicksParser.FinamStandart) { }
		public TicksLazySequentialParser(IEnumerable<string> rawStrings, ITicksParser ticksParser)
		{
			this.rawStrings = rawStrings;
			this.ticksParser = ticksParser;
		}
		#region IEnumerable
		public IEnumerator<Tick> GetEnumerator()
		{
			foreach (string line in rawStrings)
			{
				yield return ticksParser.ParseTick(line);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (string line in rawStrings)
			{
				yield return ticksParser.ParseTick(line);
			}
		}
		#endregion
	}

	public class TicksDateTimeExtractor : IEnumerable<Tick>
	{
		public IEnumerable<TickWithDateTime> ticks;
		private DateTime lastTickTime;
		public DateTime GetLastTickTime() => lastTickTime;

		public TicksDateTimeExtractor(IEnumerable<TickWithDateTime> ticks)
		{
			this.ticks = ticks;
		}

		public IEnumerator<Tick> GetEnumerator()
		{
			return ticks.Select(tick =>
			{
				lastTickTime = tick.dateTime;
				return tick.tick;
			}).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	class TicksParser : ITicksParser
	{
		char separator;
		short idIndex;
		short priceIndex;

		public static ITicksParser FinamStandart { get => FromIndexes(4, 2); }
		public static ITicksParser DamirStandart { get => FromSeparatorAndIndexes(',', 0, 2); }

		public static ITicksParser FromFunction(Func<string, Tick> parsingFunc)
		{
			return new TicksParserFromFunc(parsingFunc);
		}
		public static ITicksParser FromIndexes(short idIndex, short priceIndex)
		{
			return new TicksParser(';', idIndex, priceIndex);
		}
		public static ITicksParser FromSeparatorAndIndexes(char separator, short idIndex, short priceIndex)
		{
			return new TicksParser(separator, idIndex, priceIndex);
		}
		private TicksParser(char separator, short idIndex, short priceIndex)
		{
			this.separator = separator;
			this.idIndex = idIndex;
			this.priceIndex = priceIndex;
		}

		public Tick ParseTick(string[] data)
		{
			return new Tick(
				Convert.ToInt64(data[idIndex]),
				0,
				(double)Convert.ToDouble(data[priceIndex], System.Globalization.CultureInfo.InvariantCulture));
		}
		public Tick ParseTick(string line)
		{
			return ParseTick(line.Split(separator, StringSplitOptions.RemoveEmptyEntries));
		}
		class TicksParserFromFunc : ITicksParser
		{
			Func<string, Tick> parsingFunc;

			public TicksParserFromFunc(Func<string, Tick> parsingFunc)
			{
				this.parsingFunc = parsingFunc;
			}

			public Tick ParseTick(string line)
			{
				return parsingFunc(line);
			}
		}
	}
	class TicksWithDateTimeParser : ITickWithDateTimeParcer
	{
		public DateTime Time { get; private set; }
		char separator;
		short idIndex;
		short priceIndex;
		short dateTimeIndex;

		public static ITickWithDateTimeParcer DamirStandart { get => FromSeparatorAndIndexes(',', 0, 2, 1); }

		public static ITickWithDateTimeParcer FromFunction(Func<string, TickWithDateTime> parsingFunc)
		{
			return new TicksParserFromFunc(parsingFunc);
		}
		public static ITickWithDateTimeParcer FromIndexes(short idIndex, short priceIndex, short dateTimeIndex)
		{
			return new TicksWithDateTimeParser(';', idIndex, priceIndex, dateTimeIndex);
		}
		public static ITickWithDateTimeParcer FromSeparatorAndIndexes(char separator, short idIndex, short priceIndex, short dateTimeIndex)
		{
			return new TicksWithDateTimeParser(separator, idIndex, priceIndex, dateTimeIndex);
		}
		public static TickWithDateTime ParseTickDamir(string line)
		{
			string[] data = line.Split(',');
			return new(
				new(
					Convert.ToInt64(data[0]),
					0,
					Convert.ToDouble(data[2])),
				new(Convert.ToInt64(data[1] + "0000000"))
				);
		}
		private TicksWithDateTimeParser(char separator, short idIndex, short priceIndex, short dateTimeIndex)
		{
			this.separator = separator;
			this.idIndex = idIndex;
			this.priceIndex = priceIndex;
			this.dateTimeIndex = dateTimeIndex;
		}

		public TickWithDateTime ParseTick(string[] data)
		{
			return new TickWithDateTime(new Tick(
				Convert.ToInt64(data[idIndex]),
				0,
				Convert.ToDouble(data[priceIndex], System.Globalization.CultureInfo.InvariantCulture)), 
				new(Convert.ToInt32(data[dateTimeIndex])));
		}
		public TickWithDateTime ParseTick(string line)
		{
			return ParseTick(line.Split(separator, StringSplitOptions.RemoveEmptyEntries));
		}
		class TicksParserFromFunc : ITickWithDateTimeParcer
		{
			Func<string, TickWithDateTime> parsingFunc;

			public TicksParserFromFunc(Func<string, TickWithDateTime> parsingFunc)
			{
				this.parsingFunc = parsingFunc;
			}

			public TickWithDateTime ParseTick(string line)
			{
				return parsingFunc(line);
			}
		}
	}

	public interface ITicksParser
	{
		public Tick ParseTick(string line);
	}
	public interface ITickWithDateTimeParcer
	{
		public TickWithDateTime ParseTick(string line);
	}
}
