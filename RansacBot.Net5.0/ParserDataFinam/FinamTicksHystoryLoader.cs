using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacRealTime;
using System.IO;

namespace ParserDataFinam
{
	public class FinamTicksHystoryLoader
	{
		/// <summary>
		/// loads ticks using FINAM, the whole days from fromDate up to given lastID
		/// </summary>
		/// <param name="lastID"></param>
		/// <param name="fromDate"></param>
		/// <returns></returns>
		static public List<Tick> LoadFrom(long lastID, DateTime fromDate)
		{
			return ParseFromID(loadTicksOfTimePeriod(fromDate, DateTime.Now), lastID);
		}

		static public string loadTicksOfTimePeriod(DateTime from, DateTime to, string address = "https://www.finam.ru/profile/mosbirzha-fyuchersy/rts-12-21-riz1_riz1/export/")
		{
			from = DateTime.Now;
			to = DateTime.Now;
			ApiConfiguration.UsersFinamLink = address;
			Symbol symbol = Parser.InitSymbol().Result;
			LoadCommandOptions options = new(from, to, Period.T1, FileFormat.csv, DateFormat.DDMMYY, TimeFormat.HHMM, FieldSeparator.Semicolon, DecimalSeparator.None, DataFormat.DTLVI, false, false);
			return Parser.LoadData(symbol, options).Result;
		}

		/// <summary>
		/// feeds ticks in range of given dates both inclusevely, in range of given IDs both exclusevly
		/// </summary>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <param name="fromID"></param>
		/// <param name="toID"></param>
		/// <param name="handler"></param>
		static public void FeedRange(DateTime fromDate, DateTime toDate, long fromID, long toID, TickHandler handler)
		{
			string[] lines = loadTicksOfTimePeriod(fromDate, toDate).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
			Tick tick;
			for(int i = 0; i < lines.Length; i++)
			{
				tick = ParseTick(lines[i].Split(';'));
				if(tick.ID <= fromID)
				{
					continue;
				}
				if(tick.ID >= toID)
				{
					return;
				}
				handler(tick);
			}
		}

		static public Tick ParseTick(string[] line)
		{
			return new Tick(
				Convert.ToInt64(line[4]), 
				0, 
				(double)Convert.ToDouble(line[2], System.Globalization.CultureInfo.InvariantCulture));
		}

		static public List<Tick> ParseFromID(string data, long lastID)
		{
			List<Tick> list = new();
			string[] lines = data.Split('\n');
			int i = 0;
			string[] fields;
			do
			{
				fields = lines[i].Split(';');
				i++;
			} while ((double)Convert.ToDecimal(fields[2]) < lastID);
			list.Add(ParseTick(fields));
			for (; i < lines.Length; i++)
			{
				fields = lines[i].Split(';');
				list.Add(ParseTick(fields));
			}
			return list;
		}
	}
}
