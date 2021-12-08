using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace FinamDataLoader
{
	static class Parser
	{
		private const string CONST_Issue = "Finam.IssuerProfile.Main.issue";

		public static async Task<Symbol> InitSymbol()
		{
			string content = "";
			Symbol symbol = new Symbol();


			using (var client = new HttpClient())
			{
				WebRequestor webRequestor = new WebRequestor(client);
				Uri marketsFeedUrl = new Uri(ApiConfiguration.UsersFinamLink);
				content = await webRequestor.GetString(marketsFeedUrl).ConfigureAwait(false);
			}

			if (string.IsNullOrWhiteSpace(content))
				return symbol;

			var infoString = GetMarketString(content);

			if (string.IsNullOrEmpty(infoString))
				return symbol;

			var jsonArrayString = infoString.GetJsonArrayString();
			string[] data = jsonArrayString.Split(new char[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries);

			string sep = "\"";
			symbol = new Symbol()
			{
				Id = Convert.ToInt32(data[2]),
				MarketId = Convert.ToInt32(data[15]),
				MarketName = data[17].Replace(sep, ""),
				Name = data[8].Replace(sep, ""),
				SecCode = data[4].Replace(sep, "")
			};


			return symbol;
		}
		private static string GetMarketString(string content)
		{
			var lines = content.Split('\n');

			if (lines.Length <= 0)
				return string.Empty;

			return CommonParser.GetLineByKey(CONST_Issue, lines);
		}


		public static async Task<string> LoadData(Symbol symbol, LoadCommandOptions options)
		{
			var requestUrl = new QueryBuilder()
				.WithDateRange(symbol, options)
				.GetUrl(ApiConfiguration.FinamExportHost);

			using (var client = new HttpClient())
			{
				var requestor = new WebRequestor(client);
				return await requestor.GetString(requestUrl).ConfigureAwait(false);
			}
		}
	}
}
