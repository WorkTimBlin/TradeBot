using RansacsRealTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.HystoryTest
{
	class FileFeeder : TicksFeeder
	{
		public FileFeeder(string path) : base(new TicksFromFiles(path)) { }
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
		public TicksFromFiles(string path) : base(new StreamReader(path).GetStrings()) { }
		public TicksFromFiles(IEnumerable<string> paths) :
			base(Concat(GetStreamReaders(paths).Select(StringsFromStream.GetStringsFromStream)))
		{ }
		static IEnumerable<StreamReader> GetStreamReaders(IEnumerable<string> paths)
		{
			foreach (string path in paths) yield return new(path);
		}
		static IEnumerable<TSource> Concat<TSource>(IEnumerable<IEnumerable<TSource>> sources)
		{
			foreach(IEnumerable<TSource> source in sources)
			{
				foreach(TSource obj in source)
				{
					yield return obj;
				}
			}
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
