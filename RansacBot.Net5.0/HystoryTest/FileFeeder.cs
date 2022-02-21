using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.HystoryTest
{
	class FileFeeder : IProviderByParam<Tick>
	{
		public IEnumerable<Tick> ticks;//used for feeding
		private event Action<Tick> NewTick;

		public FileFeeder(string path)
		{
			ticks = new TicksLazySequentialParser(StringsFromFile(path));
		}

		IEnumerable<string> StringsFromFile(string filePath)
		{
			using StreamReader stream = new(filePath);
			while (!stream.EndOfStream)
			{
				yield return stream.ReadLine() ?? throw new DataException("string was null");
			}
		}

		public void FeedAllStandart(Action action)
		{
			int count = 0;
			foreach (Tick tick in ticks)
			{
				NewTick.Invoke(tick);
				if (count == 10000)
				{
					count = 0;
					action.Invoke();
				}
				count++;
			}
		}

		public void Subscribe(Param instrument, Action<Tick> handler)
		{
			NewTick += handler;
		}
		public void Unsubscribe(Param instrument, Action<Tick> handler)
		{
			NewTick -= handler;
		}
	}
}
