using QuikSharp.DataStructures;
using System;
using System.Globalization;
using System.IO;

namespace RansacBot
{
	/// <summary>
	/// Торговый инструмент (бумага). Адаптирован строго под фьючерсы.
	/// </summary>
	internal class Instrument
	{
		/// <summary>
		/// Код инструмента (бумаги)
		/// </summary>
		public string securityCode;
		/// <summary>
		/// Код класса инструмента (бумаги)
		/// </summary>
		public string classCode;
		/// <summary>
		/// Код клиента (номер счета)
		/// </summary>
		public string clientCode;
		/// <summary>
		/// Счет клиента
		/// </summary>
		public string accountID;
		/// <summary>
		/// Код фирмы
		/// </summary>
		public string firmID;



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
