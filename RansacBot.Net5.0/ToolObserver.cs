using RansacRealTime;
using System;
using System.IO;

namespace RansacBot.Net5._0
{
    internal static class ToolObserver
    {
        public static Tool CurrentTool { get; private set; }
        public static RansacObserver Data { get; private set; }
        public static double N { get; private set; }
        public static double PercentCloseN1 { get; private set; }

        public static void Initialization(RansacObserver ransacObserver, Tool tool, double n, double percent)
        {
            CurrentTool = tool;
            Data = ransacObserver;
            N = n;
            PercentCloseN1 = percent;
		}
		public static void Initialization(double n, double percent)
		{
			N = n;
			PercentCloseN1 = percent;
			OnlyLoad("SAVE", true);
		}

		public static void Save(string path, bool saveHystories)
		{
			Data.Vertexes.SaveStandart(path, saveHystories);

            using StreamWriter writer = new(path + @"/Metadata");

            writer.WriteLine(DateTime.Now.ToString());
            writer.WriteLine(CurrentTool.ClassCode);
            writer.WriteLine(CurrentTool.SecurityCode);
        }
		private static void OnlyLoad(string path, bool loadHystories)
		{
			DateTime dateTime;
			string classCode;
			string secCode;

			using (StreamReader reader = new(path + @"/Metadata"))
			{
				dateTime = DateTime.Parse(reader.ReadLine() ?? "");
				classCode = reader.ReadLine() ?? "";
				secCode = reader.ReadLine() ?? "";
			}

			//подгружаем из файлов
			Vertexes vertexes = new(path, loadHystories);
			MonkeyNFilter monkeyNFilter = new(N, vertexes.VertexList.Count);
			monkeyNFilter.NewVertex += vertexes.OnNewVertex; // в вершины

			CurrentTool = new Tool();
			Data = new RansacObserver(vertexes, monkeyNFilter);
		}
		private static void Load(string path, bool loadHystories)
		{
			DateTime dateTime;
			string classCode;
			string secCode;


			using (StreamReader reader = new(path + @"/Metadata"))
			{
				dateTime = DateTime.Parse(reader.ReadLine() ?? "");
				classCode = reader.ReadLine() ?? "";
				secCode = reader.ReadLine() ?? "";
			}

			Connector.Unsubscribe(classCode, secCode, Data.MonkeyNFilter.OnNewTick); // на всякий отписываем monkeyN на время загрузки
			string classCode1 = classCode;
			string secCode1 = secCode;
			TickFeeder hub = new(); //хаб для тиков, приходящих до загрузки с финама
			Connector.Subscribe(classCode, secCode, hub.OnNewTick);
			//запомнаем первый полученный тик, чтобы потом сравнивать его ID
			Tick firstGotTick = new(0, 0, 0);

			void TickHandler(Tick tick)
			{
				firstGotTick = tick;
				Connector.Unsubscribe(classCode1, secCode1, TickHandler);
			}

			Connector.Subscribe(classCode, secCode, TickHandler);
			TickFeeder finamTicks = new(); // в этот загружаются тики с финама
			hub.NewTick += finamTicks.OnNewTick; // потом тики хаба перегрузятся финамские,
			finamTicks.NewTick += Data.MonkeyNFilter.OnNewTick; // а затем по манки-Н
			DateTime begin = DateTime.Now;
			TimeSpan interval = new(0, 2, 0);

			//подгружаем из файлов
			Data.Vertexes = new(path, loadHystories);
			Data.MonkeyNFilter = new(N, Data.Vertexes.VertexList.Count);
			Data.MonkeyNFilter.NewVertex += Data.Vertexes.OnNewVertex; // в вершины
															 //ждем две минуты с момента подписки
			while (DateTime.Now - begin < interval)
			{

			}

			//загружаем тики с финама
			foreach (Tick tick in ParserDataFinam.BullshitUsage.LoadFrom(Data.Vertexes.VertexList[^1].ID, dateTime))
			{
				if (tick.ID >= firstGotTick.ID) //до тех пор, пока не достигнем текущих ID
					break;
				finamTicks.OnNewTick(tick);
			}

			//отписываем хаб, как только перекачаем тики из хаба в финамские, и подписываем финамские на новые из квика
			void OnFeededHub()
			{
				Connector.Unsubscribe(classCode1, secCode1, hub.OnNewTick);
				Connector.Subscribe(classCode1, secCode1, finamTicks.OnNewTick);
				hub.NewTick -= finamTicks.OnNewTick;
				finamTicks.RunFeedingToTheEnd(); // как только все подключено, кормим все получившеся тики в monkeyN
			}
			hub.ReachedEndOfQueue += OnFeededHub;
			
			//отписываем финамские, как только они кончатся, и подписываем напрямую monkeyN
			void OnFinamTicksEnd()
			{
				Connector.Unsubscribe(classCode1, secCode1, finamTicks.OnNewTick);
				Connector.Subscribe(classCode1, secCode1, Data.MonkeyNFilter.OnNewTick);
				finamTicks.NewTick -= Data.MonkeyNFilter.OnNewTick;
			}
			
			finamTicks.ReachedEndOfQueue += OnFinamTicksEnd;
			//все подключено, можно перекачивать тики
			hub.RunFeedingToTheEnd();
		}
	}
}
