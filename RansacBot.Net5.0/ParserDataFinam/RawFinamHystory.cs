using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinamDataLoader;

namespace FinamDataLoader
{
	public class RawFinamHystory:IEnumerable<string>
	{
		readonly string[] tickLines;

		private int currentLineIndex;

		public string Current { get { return tickLines[currentLineIndex]; } }
		public RawFinamHystory(DateTime fromDate, DateTime toDate)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			this.tickLines = FinamTicksHystoryLoader.loadTicksOfTimePeriod(fromDate, toDate).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
		}

		public static string[] GetTickLines(DateTime fromDate, DateTime toDate)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			return FinamTicksHystoryLoader.loadTicksOfTimePeriod(fromDate, toDate).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
		}

		public IEnumerator<string> GetEnumerator()
		{
			for(int i = 0; i < tickLines.Length; i++)
			{
				yield return tickLines[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < tickLines.Length; i++)
			{
				yield return tickLines[i];
			}
		}

		public bool MoveNext()
		{
			if(currentLineIndex < tickLines.Length)
			{
				currentLineIndex++;
				return true;
			}
			return false;
		}

		public void Reset()
		{
			currentLineIndex = 0;
		}

	}
}
