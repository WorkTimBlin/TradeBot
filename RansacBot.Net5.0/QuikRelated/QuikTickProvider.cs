using QuikSharp;
using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Text;
using RansacsRealTime;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace RansacBot
{
	class QuikTickProvider : AbstractProviderByParam<AllTrade, Tick>
	{
		readonly static Quik quik = QuikContainer.Quik;
		private static QuikTickProvider _instance;

		public static QuikTickProvider GetInstance()
		{
			if(_instance == null)
			{
				_instance = new();
			}
			return _instance;
		}
		private QuikTickProvider():base()
		{
			quik.Events.OnAllTrade += sequentialProvider.OnNewT;
		}

		protected override Tick GetTOut(AllTrade trade)
		{
			return new Tick(trade.TradeNum, 0, trade.Price);
		}
		protected override string GetKey(AllTrade trade)
		{
			return trade.ClassCode + trade.SecCode;
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
