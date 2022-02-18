using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.QuikRelated
{
	static class QuikHelpFunctions
	{
		static Quik quik = QuikContainer.Quik;
		public static Order BuildOrder(Trading.Trade trade, TradeParams tradeParams, int qty)
		{
			return new Order()
			{
				ClassCode = tradeParams.classCode,
				SecCode = tradeParams.secCode,
				Account = tradeParams.accountId,
				Operation = trade.GetOperation(),
				Price = (decimal)trade.price,
				ClientCode = tradeParams.clientCode,
				Quantity = qty
			};
		}
		public static Order BuildMarketOrder(Operation operation, TradeParams tradeParams, int qty)
		{
			return new Order()
			{
				ClassCode = tradeParams.classCode,
				SecCode = tradeParams.secCode,
				Account = tradeParams.accountId,
				Operation = operation,
				Price = GetPrice(operation == Operation.Buy ? ParamNames.HIGH : ParamNames.LOW, tradeParams),
				ClientCode = tradeParams.clientCode,
				Quantity = qty
			};
		}
		public static StopOrder BuildStopOrder(Trading.TradeWithStop tradeWithStop, TradeParams tradeParams, int qty)
		{
			return new StopOrder()
			{
				Account = tradeParams.accountId,
				ClassCode = tradeParams.classCode,
				SecCode = tradeParams.secCode,
				StopOrderType = StopOrderType.StopLimit,
				Operation = tradeWithStop.stop.GetOperation(),
				Condition = tradeWithStop.GetStopCondition(),
				ConditionPrice = (decimal)tradeWithStop.stop.price,
				Price = (decimal)tradeWithStop.stop.price,
				Quantity = qty,
				ClientCode = tradeParams.clientCode
			};
		}
		public static int GetMaxAvailibleQuantity(Operation operation, TradeParams tradeParams)
		{
			return quik.Trading.CalcBuySell(
					tradeParams.classCode,
					tradeParams.secCode,
					tradeParams.clientCode,
					tradeParams.accountId,
					(double)GetPrice(operation == Operation.Buy ? ParamNames.HIGH : ParamNames.LOW, tradeParams),
					operation == Operation.Buy,
					false).Result.Qty;
		}
		public static decimal GetPrice(ParamNames checkingPriceParam, TradeParams tradeParams)
		{
			return Decimal.Parse(quik.Trading.GetParamEx(
						tradeParams.classCode,
						tradeParams.secCode,
						checkingPriceParam
						).Result.ParamValue, System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
