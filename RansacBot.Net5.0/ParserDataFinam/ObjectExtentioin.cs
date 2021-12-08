namespace FinamDataLoader
{
	static class ObjectExtensions
	{
		public static Period ToPeriod(string timeframe)
		{
			switch (timeframe)
			{
				case "Тики":
					return Period.T1;
				case "1 минута":
					return Period.M1;
				case "5 минут":
					return Period.M5;
				case "10 минут":
					return Period.M10;
				case "15 минут":
					return Period.M15;
				case "30 минут":
					return Period.M30;
				case "1 час":
					return Period.H1;
				case "1 день":
					return Period.D1;
				case "1 неделя":
					return Period.W1;
				case "1 месяц":
					return Period.MN;
				default:
					return Period.Undefined;
			}
		}
		public static DateFormat ToDateFormat(string format)
		{
			switch (format)
			{
				case "YYYYMMDD":
					return DateFormat.YYYYMMDD;
				case "YYMMDD":
					return DateFormat.YYMMDD;
				case "DDMMYY":
					return DateFormat.DDMMYY;
				case "DD/MM/YY":
					return DateFormat.DDxMMxYY;
				case "MM/DD/YY":
					return DateFormat.MMxDDxYY;
				default:
					return DateFormat.Undefined;
			}
		}
		public static TimeFormat ToTimeFormat(string format)
		{
			switch (format)
			{
				case "HHMMSS":
					return TimeFormat.HHMMSS;
				case "HHMM":
					return TimeFormat.HHMM;
				case "HH/MM/SS":
					return TimeFormat.HHxMMxSS;
				case "HH/MM":
					return TimeFormat.HHxMM;
				default:
					return TimeFormat.Undefined;
			}
		}
		public static FieldSeparator ToFieldSeparator(string sep)
		{
			switch (sep)
			{
				case "Запятая ( , )":
					return FieldSeparator.Comma;
				case "Точка ( . )":
					return FieldSeparator.Dot;
				case "Точка с запятой ( ; )":
					return FieldSeparator.Semicolon;
				case "Tab (  )":
					return FieldSeparator.Tab;
				case "Пробел (   )":
					return FieldSeparator.Space;
				default:
					return FieldSeparator.Undefined;
			}
		}
		public static DecimalSeparator ToDecimalSeparator(string sep)
		{
			switch (sep)
			{
				case "Нет":
					return DecimalSeparator.None;
				case "Точка ( . )":
					return DecimalSeparator.Dot;
				case "Запятая ( , )":
					return DecimalSeparator.Comma;
				case "Пробел (   )":
					return DecimalSeparator.Space;
				case "Кавычка ( ` )":
					return DecimalSeparator.Quote;
				default:
					return DecimalSeparator.Undefined;
			}
		}
		public static DataFormat ToDataFormat(string format)
		{
			switch (format)
			{
				case "TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL":
					return DataFormat.TPDTOHLCV;
				case "TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE":
					return DataFormat.TPDTOHLC;
				case "TICKER, PER, DATE, TIME, CLOSE, VOL":
					return DataFormat.TPDTCV;
				case "TICKER, PER, DATE, TIME, CLOSE":
					return DataFormat.TPDTC;
				case "DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL":
					return DataFormat.TPDTOHLCV;
				case "TICKER, PER, DATE, TIME, LAST, VOL":
					return DataFormat.TPDTLV;
				case "TICKER, DATE, TIME, LAST, VOL":
					return DataFormat.TDTLV;
				case "TICKER, DATE, TIME, LAST":
					return DataFormat.TDTL;
				case "DATE, TIME, LAST, VOL":
					return DataFormat.DTLV;
				case "DATE, TIME, LAST":
					return DataFormat.DTL;
				case "DATE, TIME, LAST, VOL, ID":
					return DataFormat.DTLVI;
				case "DATE, TIME, LAST, VOL, ID, OPER":
					return DataFormat.DTLVIO;
				default:
					return DataFormat.Undefined;
			}
		}
		public static FileFormat ToFileFormat(string format)
		{
			switch (format)
			{
				case "txt":
					return FileFormat.txt;
				case "csv":
					return FileFormat.csv;
				default:
					return FileFormat.Undefined;
			}
		}

		public static int GetInt(this string stringValue, int defaultValue = 0)
		{
			if (string.IsNullOrEmpty(stringValue))
				return defaultValue;

			return int.TryParse(stringValue, out int res) ? res : defaultValue;
		}
	}
}

