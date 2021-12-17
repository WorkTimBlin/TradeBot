using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinamDataLoader
{
	internal class QueryBuilder
	{
		private readonly Query query;

		public QueryBuilder()
		{
			this.query = new Query();
		}

		public Uri GetUrl(string hostUrl)
		{
			var baseUrl = hostUrl;
			return this.query.GetUrl(baseUrl);
		}

		public QueryBuilder WithDateRange(Symbol symbol, LoadCommandOptions options)
		{
			this.query.AddDateFrom(options.From);
			this.query.AddDateTo(options.To);
			this.query.AddPeriod(options.TimeFrame);
			this.query.AddDataFormat(options.DataFormat);
			this.query.AddDateFormat(options.DateFormat);
			this.query.AddTimeFormat(options.TimeFormat);
			this.query.AddFieldSeparator(options.FieldSeparator);
			this.query.AddDecimalSeparator(options.DecimalSeparator);
			this.query.AddFileName("table", FileExtension.Txt);
			this.query.FillEmptyPeriods(options.Fill);
			this.query.AddHeader(options.Header);

			this.query.AddMarket(symbol.MarketId);
			this.query.AddTicker(symbol.Id, symbol.SecCode);

			return this;
		}
	}
}
