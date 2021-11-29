using QuikSharp;
using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Text;
using RansacRealTime;

namespace RansacBot.Net5._0
{
    static class Connector
	{
		public static readonly Quik quik;
		public delegate void NewPriceHandler(double price);
		public static NewPriceHandler NewPrice;
		private static readonly Dictionary<string, NewTickHandler> recievers = new();

		static Connector()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			quik = new Quik();
			quik.Events.OnAllTrade += OnNewTrade;
		}

		static public void Subscribe(string classCode, string secCode, NewTickHandler handler)
		{
			if(recievers.ContainsKey(classCode + secCode))
			{
				recievers[classCode + secCode] += handler;
			}
			else
			{
				recievers.Add(classCode + secCode, handler);
			}
		}

		static public void Unsubscribe(string classCode, string secCode, NewTickHandler handler)
		{
			if (recievers.ContainsKey(classCode + secCode))
			{
				recievers[classCode + secCode] -= handler;
			}
		}

		static public void OnNewTrade(AllTrade trade)
		{
			if (recievers.TryGetValue(trade.ClassCode + trade.SecCode, out NewTickHandler handler))
			{
				handler?.Invoke(new Tick(trade.TradeNum, 0, trade.Price));
				NewPrice?.Invoke(trade.Price);
			}
		}
	}
}
