using QuikSharp.DataStructures;
using System;
using System.Globalization;
using System.IO;

namespace RansacBot
{
	/// <summary>
	/// Торговый инструмент (бумага). Адаптирован строго под фьючерсы.
	/// </summary>
	class Param
	{
		/// <summary>
		/// Код класса инструмента (бумаги)
		/// </summary>
		public readonly string classCode;
		/// <summary>
		/// Код инструмента (бумаги)
		/// </summary>
		public readonly string secCode;

		private const string stdFileName = "param.csv";

		public Param(string classCode, string securityCode)
		{
			this.classCode = classCode;
			this.secCode = securityCode;
		}

		/// <summary>
		/// загружает из файла сохраняемые параметры инструмента
		/// </summary>
		/// <param name="path"></param>
		public static Param GetParamFromFile(string path, string filename = stdFileName)
		{
			using StreamReader reader = new(path + @"\" + filename);
			string classCode = (reader.ReadLine() ?? throw new Exception("can't read class code")).Split(';')[1];
			string securityCode = (reader.ReadLine() ?? throw new Exception("can't read sec code")).Split(';')[1];
			return new(classCode, securityCode);
		}

		public void SaveStandart(string path, string fileName = stdFileName)
		{
			using(StreamWriter writer = new(path + @"\" + fileName))
			{
				writer.WriteLine("classCode;" + classCode);
				writer.WriteLine("securityCode;" + secCode);
			}
		}

		public static implicit operator Param(TradeParams tradeParams)
		{
			return new(tradeParams.classCode, tradeParams.secCode);
		}

		/* конструкторы Дамира
		/// <summary>
		/// Базовый конструктор. Принимает код класса инструмента (Фьючерсы: "SPBFUT") и тикер(SecCode) инструмента. (RTS например имеет тикер - RIZ1). <br/>
		/// А также ClientCode - это номер счета клиента.
		/// </summary>
		/// <param name="secCode"></param>
		/// <param name="classCode"></param>
		public Instrument(string secCode, string clientCode, string accountId, string firmId, string classCode = "SPBFUT")
		{
			securityCode = secCode;
			this.clientCode = clientCode;
			this.classCode = classCode;
			accountID = accountId;
			firmID = firmId;

			SetBaseParam();
			SetGOInfo();
		}

		/// <summary>
		/// Базовый конструктор. Принимает код класса инструмента (Фьючерсы: "SPBFUT") и тикер(SecCode) инструмента. (RTS например имеет тикер - RIZ1). <br/>
		/// А также ClientCode - это номер счета клиента.
		/// </summary>
		/// <param name="secCode"></param>
		/// <param name="classCode"></param>
		public Instrument(string secCode)
		{
			securityCode = secCode;
			clientCode = Connector.quik.Class.GetClientCode().Result;
			classCode = "SPBFUT";
			accountID = Connector.quik.Class.GetTradeAccount(classCode).Result;
			firmID = Connector.quik.Class.GetClassInfo(classCode).Result.FirmId;

			SetBaseParam();
			SetGOInfo();
		}

		/// <summary>
		/// Тестовый конструктор.
		/// </summary>
		public Instrument()
		{
			name = "РТС";
			securityCode = "RTS";
			classCode = "ClassCode";
			clientCode = "ClientCode";
			accountID = "AccountID";
			firmID = "FirmID";
			priceAccuracy = 0;
			step = 10;
			stepPrice = 15;
			initialMarginBuy = 31000;
			initialMarginSell = 31000;
		}
		*/
	}
}
