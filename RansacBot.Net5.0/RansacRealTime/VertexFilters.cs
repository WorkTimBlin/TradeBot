namespace RansacRealTime
{
    public delegate void VertexHandler(Tick tick, VertexType vertexType);
	public delegate void TickHandler(Tick tick);

	public interface IVertexFinder
	{
		void OnNewTick(Tick tick);
		public event VertexHandler NewVertex;
	}

	public interface IVertexFilter
	{
		void OnNewVertex(Tick tick, VertexType vertexType);
		public event VertexHandler NewVertex;
	}

	public class MonkeyNFilter : IVertexFinder
	{
		readonly double n;
		int count = 0;
		Tick max;
		Tick min;
		Tick last;
		Tick lastReturned;

		public MonkeyNFilter(double n)
		{
			this.n = n;
			OnNewTickChooser = OnNewTick0;
		}

		public MonkeyNFilter(double n, Tick last)
		{
			this.n = n;
			this.count = last.VERTEXINDEX + 1;
			this.lastReturned = last;
			this.max = last;
			this.min = last;
			this.last = last;
			OnNewTickChooser = OnNewTick1;
		}

		public event VertexHandler NewVertex;

		public delegate void DelOnNewTick(Tick tick);

		private DelOnNewTick OnNewTickChooser;

		private void RaiseNewVertexEvent(Tick tick, VertexType vertexType)
		{
			if (tick.Equals(lastReturned))
			{
				return;
			}
			lastReturned = tick;
			tick = new Tick(tick.ID, count, tick.PRICE);
			count++;
			NewVertex?.Invoke(tick, vertexType);
		}

		public void OnNewTick(Tick tick)
		{
			OnNewTickChooser(tick);
		}

		private void OnNewTick0(Tick tick)
		{
			//Console.WriteLine("first tick!");
			max = tick;
			min = tick;
			this.OnNewTickChooser = OnNewTick1;
		}

		private void OnNewTick1(Tick tick)
		{
			if (tick.PRICE > max.PRICE)
			{
				max = tick;
			}
			if (tick.PRICE < min.PRICE)
			{
				min = tick;
			}
			if (tick.PRICE >= min.PRICE + n)
			{
				last = tick;
				OnNewTickChooser = OnNewTickSearchHigh;
				RaiseNewVertexEvent(min, VertexType.Low);
			}
			if (tick.PRICE <= max.PRICE - n)
			{
				last = tick;
				OnNewTickChooser = OnNewTickSearchLow;
				RaiseNewVertexEvent(max, VertexType.High);
			}
		}

		private void OnNewTickSearchHigh(Tick tick)
		{
			if (tick.PRICE > max.PRICE)
			{
				max = tick;
				if (tick.PRICE >= last.PRICE + n)
				{
					last = tick;
					RaiseNewVertexEvent(tick, VertexType.Monkey);
				}
			}
			if (tick.PRICE <= max.PRICE - n)
			{
				last = tick;
				min = tick;
				OnNewTickChooser = OnNewTickSearchLow;
				RaiseNewVertexEvent(max, VertexType.High);
			}
		}

		private void OnNewTickSearchLow(Tick tick)
		{
			if (tick.PRICE < min.PRICE)
			{
				min = tick;
				if (tick.PRICE <= last.PRICE - n)
				{
					last = tick;
					RaiseNewVertexEvent(tick, VertexType.Monkey);
				}
			}
			if (tick.PRICE >= min.PRICE + n)
			{
				last = tick;
				max = tick;
				OnNewTickChooser = OnNewTickSearchHigh;
				RaiseNewVertexEvent(min, VertexType.Low);
			}
		}
	}

}
