using QuikSharp.DataStructures;
using System;
using System.Globalization;
using System.IO;

namespace RansacBot
{
	/// <summary>
	/// Торговый инструмент (бумага). Адаптирован строго под фьючерсы.
	/// </summary>
	class Instrument
	{
		/// <summary>
		/// Код инструмента (бумаги)
		/// </summary>
		public readonly string securityCode;
		/// <summary>
		/// Код класса инструмента (бумаги)
		/// </summary>
		public readonly string classCode;
		/// <summary>
		/// Код клиента (номер счета)
		/// </summary>
		public readonly string clientCode;
		/// <summary>
		/// Счет клиента
		/// </summary>
		public readonly string accountID;
		/// <summary>
		/// Код фирмы
		/// </summary>
		public readonly string firmID;

		private const string stdFileName = "instrument.csv";

		public Instrument(string securityCode, string classCode, string clientCode, string accountID, string firmID)
		{
			this.securityCode = securityCode;
			this.classCode = classCode;
			this.clientCode = clientCode;
			this.accountID = accountID;
			this.firmID = firmID;
		}

		/// <summary>
		/// загружает из файла сохраняемые параметры инструмента
		/// </summary>
		/// <param name="path"></param>
		public Instrument(string path, string filename = stdFileName)
		{
			using (StreamReader reader = new(path + @"\" + filename))
			{
				classCode = reader.ReadLine().Split(';')[1];
				securityCode = reader.ReadLine().Split(';')[1];
				clientCode = reader.ReadLine().Split(';')[1];
				accountID = reader.ReadLine().Split(';')[1];
				firmID = reader.ReadLine().Split(';')[1];
			}
		}

		public void SaveStandart(string path, string fileName = stdFileName)
		{
			using(StreamWriter writer = new(path + @"\" + fileName))
			{
				writer.WriteLine("securityCode;" + securityCode);
				writer.WriteLine("classCode;" + classCode);
				writer.WriteLine("clientCode;" + clientCode);
				writer.WriteLine("accountID;" + accountID);
				writer.WriteLine("firmID;" + firmID);
			}
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
