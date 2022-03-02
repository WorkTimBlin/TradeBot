using RansacsRealTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacBot;

namespace RansacBot.HystoryTest
{
	class FileFeeder : TicksFeeder
	{
		public FileFeeder(string path) : base(new TicksFromFiles(path, TicksParser.FinamStandart)) { }
	}

	class TicksFeeder : IProviderByParam<Tick>
	{
		public event Action<Tick> NewTick;
		private IEnumerable<Tick> ticks;
		public TicksFeeder(IEnumerable<Tick> ticks)
		{
			this.ticks = ticks;
		}
		public void FeedAllStandart()
		{
			foreach(Tick tick in ticks)
			{
				NewTick.Invoke(tick);
			}
		}
		public void FeedAllStandartWithperiodicalInvoking(int period, Action action)
		{
			int count = 0;
			foreach (Tick tick in ticks)
			{
				NewTick.Invoke(tick);
				if(count >= period)
				{
					count = 0;
					action?.Invoke();
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

	class TicksFromFiles : TicksLazySequentialParser
	{
		public TicksFromFiles(string path, ITicksParser ticksParser) : 
			base(new StreamReader(path).GetStrings(), ticksParser) { }
		public TicksFromFiles(IEnumerable<string> paths, ITicksParser ticksParser) :
			base(GetStreamReaders(paths).Select(StringsFromStream.GetStringsFromStream).Concat(), ticksParser)
		{ }
		static IEnumerable<StreamReader> GetStreamReaders(IEnumerable<string> paths)
		{
			foreach (string path in paths) yield return new(path);
		}
	}

	static class StringsFromStream
	{
		public static IEnumerable<string> GetStrings(this StreamReader stream)
		{
			return GetStringsFromStream(stream);
		}
		public static IEnumerable<string> GetStringsFromStream(StreamReader stream)
		{
			using StreamReader stream1 = stream;
			while (!stream1.EndOfStream)
			{
				yield return stream1.ReadLine() ?? throw new Exception("somehow reached end of stream");
			}
		}
	}
}
