using RansacRealTime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	class ObservingSession
	{
		public readonly Instrument instrument;
		public readonly RansacsSession ransacs;
		private SessionMetadata metadata;

		/// <summary>
		/// starts a new session on given instrument without any loading
		/// </summary>
		/// <param name="instrument"></param>
		/// <param name="N">N from monkeyN</param>
		public ObservingSession(Instrument instrument, int N)
		{
			this.instrument = instrument;
			FillInstrument();
			this.ransacs = new(N);
			Connector.Subscribe(instrument.classCode, instrument.securityCode, this.ransacs.OnNewTick);
		}

		public void AddNewRansacsCascade(TypeSigma typeSigma, double percentile = 90)
		{
			new RansacsCascade(this.ransacs.vertexes, typeSigma, percentile);
		}

		public void Save(string path)
		{
			ransacs.
		}

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
			Connector.FillInstrument(instrument);
		}

		class SessionMetadata
		{
			public DateTime dateOfLastSave;

			public void Save(string path)
			{
				using(StreamWriter writer = new(path + @"/metadata"))
				{
					writer.WriteLine
				}
			}
		}
	}
}
