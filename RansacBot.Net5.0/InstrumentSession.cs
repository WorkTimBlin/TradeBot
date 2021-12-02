using RansacRealTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTesting
{
	class InstrumentSession
	{
		Instrument instrument;
		RansacsSession ransacs;


		/// <summary>
		/// загружает из файла сохраняемые параметры инструмента
		/// </summary>
		/// <param name="path"></param>
		private void LoadUnfinishedInstrument(string path)
		{
			using (StreamReader reader = new(path + @"/instrument"))
			{
				instrument.classCode = reader.ReadLine().Split(';')[1];
				instrument.securityCode = reader.ReadLine().Split(';')[1];
				instrument.clientCode = reader.ReadLine().Split(';')[1];
				instrument.accountID = reader.ReadLine().Split(';')[1];
				instrument.firmID = reader.ReadLine().Split(';')[1];
			}
		}

		/// <summary>
		/// заполняет инструмент, обращаясь к коннектору
		/// </summary>
		/// <param name="path"></param>
		private void FillInstrument()
		{
			Connector.FillInstrument(ref instrument);
		}

		class SessionMetaData
		{
			public DateTime dateOfLastSave;

			public void Save()
			{

			}
		}
	}
}
