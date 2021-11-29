using System;

namespace ParserDataFinam
{
    public class LoadCommandOptions
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public Period TimeFrame { get; private set; }
        public FileFormat FileFormat { get; private set; }
        public DateFormat DateFormat { get; private set; }
        public TimeFormat TimeFormat { get; private set; }
        public FieldSeparator FieldSeparator { get; private set; }
        public DecimalSeparator DecimalSeparator { get; private set; }
        public DataFormat DataFormat { get; private set; }
        public bool Header { get; private set; }
        public bool Fill { get; private set; }


        public LoadCommandOptions(DateTime from, DateTime to, Period timeframe, FileFormat fileformat, DateFormat dateFormat, TimeFormat timeFormat, FieldSeparator fieldSeparator, DecimalSeparator decimalSeparator, DataFormat dataFormat, bool header, bool fill)
        {
            From = from;
            To = to;
            TimeFrame = timeframe;
            FileFormat = fileformat;
            DateFormat = dateFormat;
            TimeFormat = timeFormat;
            FieldSeparator = fieldSeparator;
            DecimalSeparator = decimalSeparator;
            DataFormat = dataFormat;
            Header = header;
            Fill = fill;
        }


        /// <summary>
        /// Проверяет правильность и заполнение полей.
        /// </summary>
        /// <returns></returns>
        public bool IsValue()
        {
            if (TimeFrame == Period.Undefined || DateFormat == DateFormat.Undefined || TimeFormat == TimeFormat.Undefined || FieldSeparator == FieldSeparator.Undefined || DecimalSeparator == DecimalSeparator.Undefined || DataFormat == DataFormat.Undefined)
                return false;

            if (From > To)
                return false;

            return true;
        }
    }
}

