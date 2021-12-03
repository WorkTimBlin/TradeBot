using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RansacRealTime;

namespace RansacBotPrototypes
{
	static class SaveLoadManager
	{
		static float N = 100;
		public static MonkeyNFilter monkeyNFilter;
		public static void Save(string path, in Vertexes vertexes, bool saveHystories, string classSode, string secCode)
		{
			vertexes.SaveStandart(path, saveHystories);
			using (StreamWriter writer = new(path + @"/Metadata"))
			{
				writer.WriteLine(DateTime.Now.ToString());
				writer.WriteLine(classSode);
				writer.WriteLine(secCode);
			}
		}

		public static void Load(string path, out Vertexes vertexes, bool loadHystories, out string classCode, out string secCode)
		{
			DateTime dateTime;
			using (StreamReader reader = new(path + @"/Metadata"))
			{
				dateTime = DateTime.Parse(reader.ReadLine()??"");
				classCode = reader.ReadLine()??"";
				secCode = reader.ReadLine()??"";
			}
			Connector.Unsubscribe(classCode, secCode, monkeyNFilter.OnNewTick); // на всякий отписываем monkeyN на время загрузки
			string classCode1 = classCode;
			string secCode1 = secCode;
			TickFeeder hub = new(); //хаб для тиков, приходящих до загрузки с финама
			Connector.Subscribe(classCode, secCode, hub.OnNewTick);
			//запомнаем первый полученный тик, чтобы потом сравнивать его ID
			Tick firstGotTick = new Tick(0, 0, 0);
			void TickHandler(Tick tick)
			{
				firstGotTick = tick;
				Connector.Unsubscribe(classCode1, secCode1, TickHandler);
			}
			Connector.Subscribe(classCode, secCode, TickHandler);
			TickFeeder finamTicks = new(); // в этот загружаются тики с финама
			hub.NewTick += finamTicks.OnNewTick; // потом тики хаба перегрузятся финамские,
			finamTicks.NewTick += monkeyNFilter.OnNewTick; // а затем по манки-Н
			DateTime begin = DateTime.Now;
			TimeSpan interval = new(0, 2, 0);
			//подгружаем из файлов
			vertexes = new(path, true);
			monkeyNFilter = new(N, vertexes.VertexList[^1]);
			monkeyNFilter.NewVertex += vertexes.OnNewVertex; // в вершины
			//ждем две минуты с момента подписки
			while(DateTime.Now - begin < interval)
			{

			}
			//загружаем тики с финама
			foreach (Tick tick in ParserDataFinam.FinamHystoryFeeder.LoadFrom(vertexes.VertexList[^1].ID, dateTime))
			{
				if(tick.ID >= firstGotTick.ID) //до тех пор, пока не достигнем текущих ID
				{
					break;
				}
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
				Connector.Subscribe(classCode1, secCode1, monkeyNFilter.OnNewTick);
				finamTicks.NewTick -= monkeyNFilter.OnNewTick;
			}
			finamTicks.ReachedEndOfQueue += OnFinamTicksEnd;
			//все подключено, можно перекачивать тики
			hub.RunFeedingToTheEnd();
		}
	}
}
