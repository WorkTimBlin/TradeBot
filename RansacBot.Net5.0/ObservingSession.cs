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
		public readonly List<RansacsCascade> ransacsCascades;
		private bool isActive = false;
		private bool isUpdated = false;
		DateTime dateTimeOfLastSave;

		/// <summary>
		/// starts a new session on given instrument without any loading and immediatly subscribes ransacs to new ticks
		/// </summary>
		/// <param name="instrument"></param>
		/// <param name="N">N from monkeyN</param>
		public ObservingSession(Instrument instrument, int N)
		{
			this.instrument = instrument;
			this.ransacs = new(N);
			ransacsCascades = this.ransacs.vertexes.cascades;
			isUpdated = true;
		}

		/// <summary>
		/// Initialises Instrument and ransacs session from file
		/// </summary>
		/// <param name="path"></param>
		public ObservingSession(string path)
		{
			instrument = new(path);
			ransacs = new(path);
			ransacsCascades = ransacs.vertexes.cascades;
			dateTimeOfLastSave = LoadMetadata(path).dateTimeOfLastSave;
		}

		public void SubscribeToQuik()
		{
			if (!isUpdated) throw new Exception("Can't subscribe until is not updated!");
			if (isActive) throw new Exception("Can't subscribe while subscribed!!");
			Connector.Subscribe(instrument.classCode, instrument.securityCode, this.ransacs.OnNewTick);
			isActive = true;
		}
		public void UnsubscribeOfQuik()
		{
			if (!isActive) return;
			Connector.Unsubscribe(instrument, this.ransacs.OnNewTick);
			isActive = false;
			isUpdated = false;
		}
		public void SubscribeTo(ITickByInstrumentProvider provider)
		{
			if (!isUpdated) throw new Exception("Can't subscribe until is not updated!");
			if (isActive) throw new Exception("Can't subscribe while subscribed!!");
			provider.Subscribe(instrument, this.ransacs.OnNewTick);
			isActive = true;
		}
		public void UnsubscribeOf(ITickByInstrumentProvider provider)
		{
			if (!isActive) return;
			provider.Unsubscribe(instrument, this.ransacs.OnNewTick);
			isActive = false;
			isUpdated = false;
		}

		public void UpdateFromTicksUpToEndKeepingUpWithProviderWaitingForTime(
			IList<Tick> ticks,
			ITickByInstrumentProvider provider,
			TimeSpan time)
		{
			if (isUpdated) return;
			Queue<Tick> hub = new();
			provider.Subscribe(instrument, hub.Enqueue);
			DateTime targetDateTime = DateTime.Now + time;
			while (DateTime.Now < targetDateTime || hub.Count == 0) ;
			FeedRansacsWithTicksUpToID(
				ticks.SkipWhile((Tick tick) => tick.ID <= ransacs.vertexes.vertexList.Last().ID),
				hub.Peek().ID);
			FeedRansacsWholeQueue(hub);
			Connector.Unsubscribe(instrument.classCode, instrument.securityCode, hub.Enqueue);
			isUpdated = true;
		}
		public void UpdateFromTicksUpToEnd(IList<Tick> ticks)
		{
			if (isUpdated) return;
			IList<Tick> shit = ticks.SkipWhile((Tick tick) => tick.ID <= ransacs.vertexes.vertexList.Last().ID).ToList();
			FeedRansacsWithTicksUpToID(
				ticks.SkipWhile((Tick tick) => tick.ID <= ransacs.vertexes.vertexList.Last().ID).Append(new Tick(ticks.Last().ID + 1, 0, 0)),
				ticks[^1].ID + 1);
			isUpdated = true;
		}
		public void UpdateFromFinamAndLaunchUsingQuik()
		{
			UnsubscribeOfQuik();
			UpdateUpToNowUsingFinam();
			SubscribeToQuik();
		}
		public void AddNewRansacsCascade(TypeSigma typeSigma, double percentile = 90)
		{
			new RansacsCascade(this.ransacs.vertexes, typeSigma, percentile);
		}
		/// <summary>
		/// feeds ransacs with ticks from finam from given date
		/// ransacs should be Unsubscribed when this function is called
		/// </summary>
		/// <param name="dateTimeOfLastSave"></param>
		private void UpdateUpToNowUsingFinam()
		{
			if (isUpdated) return;
			Queue<Tick> hub = new();
			Connector.Subscribe(instrument.classCode, instrument.securityCode, hub.Enqueue);
			DateTime targetDateTime = DateTime.Now + new TimeSpan(0, 2, 0);
			while (DateTime.Now < targetDateTime || hub.Count == 0) ;
			FeedRansacsWithTicksUpToID(
				new TicksLazyParser(
					FinamDataLoader.RawFinamHystory.GetTickLines(
						dateTimeOfLastSave,
						DateTime.Now)).SkipWhile((Tick tick) => tick.ID <= ransacs.vertexes.vertexList.Last().ID),
				hub.Peek().ID);
			FeedRansacsWholeQueue(hub);
			Connector.Unsubscribe(instrument.classCode, instrument.securityCode, hub.Enqueue);
			isUpdated = true;
		}
		//TODO:delete
		/// <summary>
		/// TO DELETE!! METHOD FOR TESTS ONLY!!
		/// </summary>
		public void UpdateEmpty()
		{
			isUpdated = true;
		}

		/// <summary>
		/// Feeds ticks from finam hystory into ransacs session, stops when ID of tick from hystory equals to given
		/// Throws exception if there is no tick with such ID
		/// </summary>
		/// <param name="startDate"></param>
		public void FeedRansacsWithTicksUpToID(IEnumerable<Tick> ticksHystory, long stopID)
		{
			foreach(Tick tick in ticksHystory)
			{
				if (tick.ID == stopID) 
					return;
				this.ransacs.OnNewTick(tick);
			}
			throw new ArgumentException("hystoryDoesn't reach stopID");
		}
		public void FeedRansacsWholeQueue(Queue<Tick> ticks)
		{
			while(ticks.Count > 0)
			{
				ransacs.OnNewTick(ticks.Dequeue());
			}
		}

		public void SaveStandart(string path)
		{
			SaveMetadata(path);
			instrument.SaveStandart(path);
			ransacs.SaveStandart(path);
		}

		private const string metadataName = "metadata";
		private void SaveMetadata(string path, string fileName = metadataName)
		{
			using(StreamWriter writer = new(path + @"/" + fileName))
			{
				writer.WriteLine("dateTimeOfLastSave;" + DateTime.Now.ToString());
			}
		}

		private (DateTime dateTimeOfLastSave, object? someFool) LoadMetadata(string path, string fileName = metadataName)
		{
			using (StreamReader reader = new(path + @"/" + fileName))
			{
				return (DateTime.Parse(reader.ReadLine().Split(';', StringSplitOptions.RemoveEmptyEntries)[1]), null);
			}
		}
	}
}
