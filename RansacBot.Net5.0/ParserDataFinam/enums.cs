namespace FinamDataLoader
{
	enum Period
	{
		Undefined = 0,
		T1 = 1, // Ticks
		M1 = 2,
		M5 = 3,
		M10 = 4,
		M15 = 5,
		M30 = 6,
		H1 = 7,
		D1 = 8,
		W1 = 9, // Week
		MN = 10 // Month
	}
	enum FileFormat
	{
		Undefined = 0,
		txt = 1,
		csv = 2
	}

	enum DataFormat
	{
		Undefined = 0,
		TPDTOHLCV = 1,  // TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
		TPDTOHLC = 2,   // TICKER, PER, DATE, TIME, OPEN, HIGH, LOW, CLOSE
		TPDTCV = 3,     // TICKER, PER, DATE, TIME, CLOSE, VOL
		TPDTC = 4,      // TICKER, PER, DATE, TIME, CLOSE
		DTOHLCV = 5,    // DATE, TIME, OPEN, HIGH, LOW, CLOSE, VOL
		TPDTLV = 6,     // TICKER, PER, DATE, TIME, LAST, VOL
		TDTLV = 7,      // TICKER, DATE, TIME, LAST, VOL
		TDTL = 8,       // TICKER, DATE, TIME, LAST
		DTLV = 9,       // DATE, TIME, LAST, VOL
		DTL = 10,       // DATE, TIME, LAST
		DTLVI = 11,     // DATE, TIME, LAST, VOL, ID 
		DTLVIO = 12     // DATE, TIME, LAST, VOL, ID, OPER
	}

	enum DateFormat
	{
		Undefined = 0,
		YYYYMMDD = 1,
		YYMMDD = 2,
		DDMMYY = 3,
		DDxMMxYY = 4, // DD/MM/YY
		MMxDDxYY = 5 // MM/DD/YY
	}

	enum TimeFormat
	{
		Undefined = 0,
		HHMMSS = 1,
		HHMM = 2,
		HHxMMxSS = 3,   // HH:MM:SS
		HHxMM = 4       // HH:MM 
	}

	enum FieldSeparator
	{
		Undefined = 0,
		Comma = 1,      // ,
		Dot = 2,        // .
		Semicolon = 3,  // ;
		Tab = 4,
		Space = 5,
	}

	enum DecimalSeparator
	{
		Undefined = 0,
		None = 1,
		Dot = 2,   // .
		Comma = 3, //,
		Space = 4,
		Quote = 5  // '
	}
}
