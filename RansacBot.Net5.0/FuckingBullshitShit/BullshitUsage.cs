using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacRealTime;
using System.IO;

namespace ParserDataFinam
{
	public class BullshitUsage
	{
		static public List<Tick> LoadFrom(long lastID, DateTime from)
		{
			return ParseFromID(loadTicksOfTimePeriod(from, DateTime.Now), lastID);
		}

		static public string loadTicksOfTimePeriod(DateTime from, DateTime to)
		{
			from = DateTime.Now;
			to = DateTime.Now;
			ApiConfiguration.UsersFinamLink = "https://www.finam.ru/profile/mosbirzha-fyuchersy/rts-12-21-riz1_riz1/export/";
			Symbol symbol = Parser.InitSymbol().Result;
			LoadCommandOptions options = new(from, to, Period.T1, FileFormat.csv, DateFormat.DDMMYY, TimeFormat.HHMM, FieldSeparator.Semicolon, DecimalSeparator.Dot, DataFormat.DTLVI, false, false);
			return Parser.LoadData(symbol, options).Result;
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
			for (; i < lines.Length; i++)
			{
				fields = lines[i].Split(';');
				list.Add(ParseTick(fields));
			}
			return list;
		}

		static public Tick ParseTick(string[] line)
		{
			return new Tick(Convert.ToInt64(line[4]), 0, (double)Convert.ToDecimal(line[2]));
		}
	}
}
