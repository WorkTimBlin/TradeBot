using System;
using System.Collections.Generic;
using System.IO;

namespace RansacsRealTime
{

	public class TickFeeder
	{
		private Queue<Tick> ticks = new();
		public bool EndOfQueue { get { return !(ticks.Count > 0); } }
		public event TickHandler NewTick;
		public delegate void VoidHandler();
		public event VoidHandler ReachedEndOfQueue;

		public void RunFeedingToTheEnd()
		{
			while(!EndOfQueue)
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