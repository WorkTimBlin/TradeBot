using QuikSharp;
using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Text;

namespace RansacBot.Net5._0
{
    static class Connector
	{
		public static readonly Quik quik;
		public delegate void NewTickHandler(Tick tick);
		public delegate void NewPriceHandler(double price);
		public static NewPriceHandler NewPrice;
		private static Dictionary<string, NewTickHandler> recievers = new();

		static Connector()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			quik = new Quik();
			quik.Events.OnAllTrade += OnNewTrade;
		}

		static public void Subscribe(string classCode, string secCode, NewTickHandler handler)
		{
			recievers.Add(classCode + secCode, handler);
		}


		static public void OnNewTrade(AllTrade trade)
		{
			if (recievers.TryGetValue(trade.ClassCode + trade.SecCode, out NewTickHandler handler) && handler != null)
			{
				handler(new Tick(trade.TradeNum, 0, (float)trade.Price));
				NewPrice(trade.Price);
			}
		}
	}
}
