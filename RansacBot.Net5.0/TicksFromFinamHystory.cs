using ParserDataFinam;
using RansacRealTime;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BotTesting
{
	class TicksFromFinamHystory : IEnumerable<Tick>
	{
		private RawFinamHystory rawStrings;
		public TicksFromFinamHystory(DateTime fromDate, DateTime toDate)
		{
			rawStrings = new(fromDate, toDate);
		}

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
}
