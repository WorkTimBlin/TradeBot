using QuikSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacBot;
using QuikSharp.DataStructures;

namespace RansacBot.QuikRelated
{
	class QuikOrderBookProvider : AbstractProviderByParam<OrderBook, OrderBook>
	{
		Quik quik = QuikContainer.Quik;

		QuikOrderBookProvider():base()
		{
			quik.Events.OnQuote += sequentialProvider.OnNewT;
		}

		public new void Subscribe(Param param, Action<OrderBook> action)
		{
			quik.OrderBook.Subscribe(param.classCode, param.secCode);
			base.Subscribe(param, action);
		}

		protected override string GetKey(OrderBook tin)
		{
			return tin.class_code + tin.sec_code;
		}

		protected override OrderBook GetTOut(OrderBook tin)
		{
			return tin;
		}
	}
}
