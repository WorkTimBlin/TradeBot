using RansacRealTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RansacBot
{
	internal class InstrumentObserver
    {
		private Instrument instrument;
		private RansacsSession ransacsObserver;

		public DateTime dateTimeOfSaving;


        public void Initialize(RansacsSession ransacObserver, Instrument instrument)
        {
            this.instrument = instrument;
            ransacsObserver = ransacObserver;
		}
		//TODO: rewrite Save
		/*
		public void Save(string path, bool saveHystories)
		{
			ransacsObserver.vertexes.SaveStandart(path + @"/RansacsObserver", saveHystories);
			SaveMetadata(path);
        }
		*/

		private void SaveInstrument(string path)
		{
			using StreamWriter writer = new(path + @"/instrument");
            writer.WriteLine("код класса инструмента;" + instrument.classCode.ToString());
            writer.WriteLine("код инструмента;" + instrument.securityCode.ToString());
			writer.WriteLine("код клиента(?)" + instrument.clientCode.ToString());
			writer.WriteLine("ID аккаунта(?)" + instrument.accountID.ToString());
			writer.WriteLine("ID фирмы (?)" + instrument.firmID.ToString());
		}

		/// <summary>
		/// инициализация ransacObserver из сохраненного в файлах
		/// </summary>
		/// <param name="path"></param>
		/// <param name="loadHystories">подгружать ли уровни ранзаков, или только вершины</param>
		private void LoadRansacObserver(string path, bool loadHystories)
		{
			ransacsObserver = new RansacsSession(path + @"/RansacsObserver", loadHystories);
		}

		private void SaveMetadata(string path)
		{
            using StreamWriter writer = new(path + @"/metadata.csv");
            writer.WriteLine("дата и время сохранения;", DateTime.Now.ToString());
		}

		private void LoadMetadata(string path)
		{
			using (StreamReader reader = new(path + @"/metadata"))
			{
				dateTimeOfSaving = DateTime.Parse((reader.ReadLine() ?? "").Split(';')[1]);
			}
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
		/*
		private void FeedLostTicksFromFinam(TimeSpan interval)
		{
			Queue<Tick> hub = new(); //хаб для тиков, приходящих до загрузки с финама
			Connector.Subscribe(instrument.classCode, instrument.securityCode, hub.OnNewTick);// подписываем хаб на новые тики
			hub.NewTick += finamTicks.OnNewTick; // потом тики хаба перегрузятся в финамские
			DateTime begin = DateTime.Now;
			//ждем интервал времени с момента подписки
			//TODO: сейчас ожидание фризит форму. выяснить, как ожидать без фриза
			//(?)Task.Delay(interval);
			while (DateTime.Now - begin < interval && hub.EndOfQueue) { }
			Tick firstGotTick = hub.Peek();
			foreach (Tick tick in ParserDataFinam.FinamHystoryLoader.LoadFrom(vertexes.VertexList[^1].ID, dateTime))
			{
				if (tick.ID >= firstGotTick.ID) //до тех пор, пока не достигнем текущих ID
					break;
				finamTicks.OnNewTick(tick);
			}
		}
		*/

		//TODO: delete and replace
		/*
		private void LoadUsingFinamUpToDate(string path, bool loadHystories)
		{
			OnlyLoad(path, loadHystories);
			string classCode = instrument.classCode;
			string secCode = instrument.securityCode;
			Vertexes vertexes = ransacsObserver.vertexes;
			MonkeyNFilter monkeyNFilter = ransacsObserver.monkeyNFilter;


			Connector.Unsubscribe(classCode, secCode, monkeyNFilter.OnNewTick); // на всякий отписываем monkeyN на время загрузки
			string classCode1 = classCode;
			string secCode1 = secCode;
			TickFeeder hub = new(); //хаб для тиков, приходящих до загрузки с финама
			Connector.Subscribe(classCode, secCode, hub.OnNewTick);
			//запомнаем первый полученный тик, чтобы потом сравнивать его ID
			Tick firstGotTick = new(0, 0, 0);
			LOGGER.Trace("Load(): создали хаб");

			void TickHandler(Tick tick)
			{
				firstGotTick = tick;
				Connector.Unsubscribe(classCode1, secCode1, TickHandler);
			}

			Connector.Subscribe(classCode, secCode, TickHandler);
			TickFeeder finamTicks = new(); // в этот загружаются тики с финама
			hub.NewTick += finamTicks.OnNewTick; // потом тики хаба перегрузятся в финамские
			finamTicks.NewTick += monkeyNFilter.OnNewTick; // а затем по манки-Н
			DateTime begin = DateTime.Now;
			TimeSpan interval = new(0, 2, 0);

			//подгружаем из файлов
			vertexes = new(path, loadHystories);
			monkeyNFilter = new(N, vertexes.VertexList[^1]);
			monkeyNFilter.NewVertex += vertexes.OnNewVertex; // в вершины

			LOGGER.Trace("Load(): Ожидаем 2 минуты");
			//ждем две минуты с момента подписки
			while (DateTime.Now - begin < interval)
			{

			}

			//загружаем тики с финама
			foreach (Tick tick in ParserDataFinam.FinamHystoryLoader.LoadFrom(vertexes.VertexList[^1].ID, dateTime))
			{
				if (tick.ID >= firstGotTick.ID) //до тех пор, пока не достигнем текущих ID
					break;
				finamTicks.OnNewTick(tick);
			}

			LOGGER.Trace("Load(): Загрузили тики с финама");

			//отписываем хаб, как только перекачаем тики из хаба в финамские, и подписываем финамские на новые из квика
			void OnFeededHub()
			{
				Connector.Unsubscribe(classCode1, secCode1, hub.OnNewTick);
				Connector.Subscribe(classCode1, secCode1, finamTicks.OnNewTick);
				hub.NewTick -= finamTicks.OnNewTick;
				finamTicks.RunFeedingToTheEnd(); // как только все подключено, кормим все получившеся тики в monkeyN
				LOGGER.Trace("Load(): OnFeededHub - сработало");
			}
			hub.ReachedEndOfQueue += OnFeededHub;

			

			//отписываем финамские, как только они кончатся, и подписываем напрямую monkeyN
			void OnFinamTicksEnd()
			{
				Connector.Unsubscribe(classCode1, secCode1, finamTicks.OnNewTick);
				Connector.Subscribe(classCode1, secCode1, monkeyNFilter.OnNewTick);
				finamTicks.NewTick -= monkeyNFilter.OnNewTick;
				LOGGER.Trace("Load(): OnFinamTicksEnd - сработало");
			}
			
			finamTicks.ReachedEndOfQueue += OnFinamTicksEnd;
			LOGGER.Trace("Load(): почти конец");
			//все подключено, можно перекачивать тики
			hub.RunFeedingToTheEnd();

			ransacsObserver = new RansacObserver(vertexes, monkeyNFilter);
		}
		*/
	}
}
