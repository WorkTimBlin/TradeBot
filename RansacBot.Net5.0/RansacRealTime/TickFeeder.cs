using System;
using System.Collections.Generic;
using System.IO;

namespace RansacRealTime
{

	public class TickFeeder
	{
		private Queue<Tick> ticks;
		public bool EndOfQueue { get { return ticks.Count > 0; } }
		public event NewTickHandler NewTick;

		public void RunFeedingToTheEnd()
		{
			while(ticks.Count > 0)
			{
				FeedOne();
			}
		}

		public void FeedOne()
		{
			NewTick?.Invoke(ticks.Dequeue());
		}

		public void OnNewTick(Tick tick)
		{
			ticks.Enqueue(tick);
		}
	}
}