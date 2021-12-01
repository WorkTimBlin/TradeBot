using QuikSharp;
using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Text;
using RansacRealTime;
using System;
using System.Globalization;

namespace RansacBot.Net5._0
{
    static class Connector
	{
		public static readonly Quik quik;
		public delegate void NewPriceHandler(double price);
		public static NewPriceHandler NewPrice;
		private static readonly Dictionary<string, NewTickHandler> recievers = new();

		private static readonly Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

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

		static public Instrument GetInstrument(string classCode, string secCode, string clientCode, string accountId, string firmId)
		{
			var baseParam = GetSecurityInfo(classCode, secCode);
			var IMInfo = GetInitialMarginInfo(classCode, secCode);
			return new()
			{
				securityCode = secCode,
				clientCode = clientCode,
				classCode = classCode,
				accountID = accountId,
				firmID = firmId,
				name = baseParam.name,
				step = baseParam.step,
				priceAccuracy = baseParam.priceAccuracy,
				initialMarginBuy = IMInfo.initialMarginBuy,
				initialMarginSell = IMInfo.initialMarginSell,
				stepPrice = IMInfo.stepPrice
			};
		}

		//TODO: fix Get and FillInstrument
		static public void FillInstrument(ref Instrument instrument)
		{
			instrument = GetInstrument(instrument.classCode, instrument.securityCode, instrument.clientCode, instrument.accountID, instrument.firmID);
		}

		/// <summary>
		/// имя, шаг цены и ее точность
		/// </summary>
		private static (string name, double step, int priceAccuracy) GetSecurityInfo(string classCode, string securityCode)
		{
			if (classCode != null && classCode != "")
			{
				SecurityInfo infoTool = Connector.quik.Class.GetSecurityInfo(classCode, securityCode).Result;

				if (infoTool != null)
				{
					string name = infoTool.Name;
					double step = infoTool.MinPriceStep;
					int priceAccuracy = infoTool.Scale;
					return (name, step, priceAccuracy);
				}
				else
				{
					throw new Exception(
						"Tool.SetBaseParam() - Не удалось загрузить информацию об инструменте " +
						securityCode);
				}
			}
			else
			{
				throw new Exception(
					"Tool.SetBaseParam() - Код класса '" +
					classCode +
					"' инструмента не обнаружен.");
			}
		}

		/// <summary>
		/// параметры ГО и стоимости шага цены. 
		/// </summary>
		/// <param name="classCode"></param>
		/// <param name="securityCode"></param>
		/// <returns>кортеж с маржой на покупку и продажу и шаг цены</returns>
		private static (double initialMarginBuy, double initialMarginSell, double stepPrice) GetInitialMarginInfo(string classCode, string securityCode)
		{
			try
			{
				double initialMarginBuy = Convert.ToDouble(Connector.quik.Trading.GetParamEx(classCode, securityCode, ParamNames.BUYDEPO).Result.ParamValue.Replace('.', separator));
				double initialMarginSell = Convert.ToDouble(Connector.quik.Trading.GetParamEx(classCode, securityCode, ParamNames.SELLDEPO).Result.ParamValue.Replace('.', separator));
				double stepPrice = Convert.ToDouble(Connector.quik.Trading.GetParamEx(classCode, securityCode, ParamNames.STEPPRICE).Result.ParamValue.Replace('.', separator));
				return (initialMarginBuy, initialMarginSell, stepPrice);
			}
			catch (Exception ex)
			{
				throw new Exception(
					"Tool.SetGOInfo(): Exception во время загрузки ГО инструмента: " +
					ex.Message);
			}
		}

	}
}
