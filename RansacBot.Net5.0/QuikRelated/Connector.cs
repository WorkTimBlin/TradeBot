using QuikSharp;
using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Text;
using RansacsRealTime;
using System;
using System.Globalization;

namespace RansacBot
{
	class Connector : ITickByInstrumentProvider
	{
		public readonly Quik quik;
		public delegate void NewPriceHandler(double price);
		private readonly Dictionary<string, TickHandler> recievers = new();
		private static Connector _instance;


		private Connector()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			quik = new Quik();
			quik.Events.OnAllTrade += OnNewTrade;
		}
		public static Connector GetInstance()
		{
			if(_instance == null)
			{
				_instance = new();
			}
			return _instance;
		}

		public void OnNewTrade(AllTrade trade)
		{
			if (recievers.TryGetValue(trade.ClassCode + trade.SecCode, out TickHandler handler))
			{
				handler?.Invoke(new Tick(trade.TradeNum, 0, trade.Price));
			}
		}

		public void Subscribe(string classCode, string secCode, TickHandler handler)
		{
			if (recievers.ContainsKey(classCode + secCode))
			{
				recievers[classCode + secCode] += handler;
			}
			else
			{
				recievers.Add(classCode + secCode, handler);
			}
		}
		public void Subscribe(Instrument instrument, TickHandler handler)
		{
			Subscribe(instrument.classCode, instrument.securityCode, handler);
		}
		public void Unsubscribe(string classCode, string secCode, TickHandler handler)
		{
			if (recievers.ContainsKey(classCode + secCode))
			{
				recievers[classCode + secCode] -= handler ?? throw new Exception("tried to unsubscribe null");
			}
		}
		public void Unsubscribe(Instrument instrument, TickHandler handler)
		{
			Unsubscribe(instrument.classCode, instrument.securityCode, handler);
		}

		/*
		private static readonly Char separator { get; set; } = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
		/// <summary>
		/// имя, шаг цены и ее точность
		/// </summary>
		private (string name, double step, int priceAccuracy) 
			GetSecurityInfo(string classCode, string securityCode)
		{
			if (classCode != null && classCode != "")
			{
				SecurityInfo infoTool = quik.Class.GetSecurityInfo(classCode, securityCode).Result;

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
		private (double initialMarginBuy, double initialMarginSell, double stepPrice) 
			GetInitialMarginInfo(string classCode, string securityCode)
		{
			try
			{
				double initialMarginBuy = Convert.ToDouble(quik.Trading.GetParamEx(classCode, securityCode, ParamNames.BUYDEPO).Result.ParamValue.Replace('.', separator));
				double initialMarginSell = Convert.ToDouble(quik.Trading.GetParamEx(classCode, securityCode, ParamNames.SELLDEPO).Result.ParamValue.Replace('.', separator));
				double stepPrice = Convert.ToDouble(quik.Trading.GetParamEx(classCode, securityCode, ParamNames.STEPPRICE).Result.ParamValue.Replace('.', separator));
				return (initialMarginBuy, initialMarginSell, stepPrice);
			}
			catch (Exception ex)
			{
				throw new Exception(
					"Tool.SetGOInfo(): Exception во время загрузки ГО инструмента: " +
					ex.Message);
			}
		}
		*/
	}
}
