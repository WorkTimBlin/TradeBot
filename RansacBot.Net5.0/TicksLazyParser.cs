using FinamDataLoader;
using RansacRealTime;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RansacBot
{
	public class TicksLazyParser : IEnumerable<Tick>
	{
		private ICollection<string> rawStrings;
		public TicksLazyParser(ICollection<string> rawStrings)
		{
			this.rawStrings = rawStrings;
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
