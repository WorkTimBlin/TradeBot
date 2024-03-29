﻿using RansacsRealTime;
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
		public readonly Param param;
		public readonly RansacsSession ransacs;
		public readonly List<RansacsCascade> ransacsCascades;
		private bool isActive = false;
		private bool isUpdated = false;
		public readonly IProviderByParam<Tick> provider;
		DateTime dateTimeOfLastSave;

		/// <summary>
		/// starts a new session on given instrument without any loading and immediatly subscribes ransacs to new ticks
		/// </summary>
		/// <param name="instrument"></param>
		/// <param name="N">N from monkeyN</param>
		public ObservingSession(Param instrument, IProviderByParam<Tick> provider, int N)
		{
			this.param = instrument;
			this.provider = provider;
			this.ransacs = new(N);
			ransacsCascades = this.ransacs.vertexes.cascades;
			isUpdated = true;
		}
		/// <summary>
		/// Initialises Instrument and ransacs session from file
		/// </summary>
		/// <param name="path"></param>
		public ObservingSession(string path, IProviderByParam<Tick> provider)
		{
			param = Param.GetParamFromFile(path);
			this.provider = provider;
			ransacs = new(path);
			ransacsCascades = ransacs.vertexes.cascades;
			dateTimeOfLastSave = LoadMetadata(path).dateTimeOfLastSave;
		}

		public void SubscribeToProvider()
		{
			if (!isUpdated) throw new Exception("Can't subscribe until is not updated!");
			if (isActive) throw new Exception("Can't subscribe while subscribed!!");
			provider.Subscribe(param, this.ransacs.OnNewTick);
			isActive = true;
		}
		public void UnsubscribeOfProvider()
		{
			if (!isActive) return;
			provider.Unsubscribe(param, this.ransacs.OnNewTick);
			isActive = false;
			isUpdated = false;
		}
		public void UpdateFromTicksKeepingUpUsingDelayTimeAndStaySubscribed(
			ITicksHystoryGetter ticks, TimeSpan time)
		{
			if (isUpdated) return;
			Queue<Tick> hub = new();
			provider.Subscribe(param, hub.Enqueue);
			DateTime targetDateTime = DateTime.Now + time;
			while (DateTime.Now < targetDateTime || hub.Count == 0) ;
			FeedRansacsWithTicksUpToID(
				ticks.GetTicks(dateTimeOfLastSave, DateTime.Now).SkipWhile((Tick tick) => tick.ID <= ransacs.vertexes.vertexList.Last().ID),
				hub.Peek().ID);
			FeedRansacsWholeQueue(hub);
			provider.Unsubscribe(param, hub.Enqueue);
			isUpdated = true;
		}
		public void UpdateFromTicksUpToEnd(IList<Tick> ticks)
		{
			if (isUpdated) return;
			FeedRansacsWithTicks(
				ticks.SkipWhile((Tick tick) => tick.ID <= ransacs.vertexes.vertexList.Last().ID));
			isUpdated = true;
		}
		public RansacsCascade AddNewRansacsCascade(SigmaType sigmaType, double percentile = 90)
		{
			return new RansacsCascade(this.ransacs.vertexes, sigmaType, percentile);
		}

		public RansacsCascade AddNewRansacsCascade(SigmaType sigmaType, int maxLevel, double percentile = 90)
        {
			return new RansacsCascade(this.ransacs.vertexes, sigmaType, maxLevel, percentile);
        }
		//public void UpdateFromFinamAndLaunchUsingQuik()
		//{
		//	UnsubscribeOfQuik();
		//	UpdateUpToNowUsingFinam();
		//	SubscribeToQuik();
		//}
		/// <summary>
		/// feeds ransacs with ticks from finam from given date
		/// ransacs should be Unsubscribed when this function is called
		/// </summary>
		/// <param name="dateTimeOfLastSave"></param>
		private void UpdateUpToNowUsingFinam()
		{
			if (isUpdated) return;
			Queue<Tick> hub = new();
			QuikTickProvider.GetInstance().Subscribe(param.classCode, param.secCode, hub.Enqueue);
			DateTime targetDateTime = DateTime.Now + new TimeSpan(0, 2, 0);
			while (DateTime.Now < targetDateTime || hub.Count == 0) ;
			FeedRansacsWithTicksUpToID(
				new TicksLazyParser(
					FinamDataLoader.RawFinamHystory.GetTickLines(
						dateTimeOfLastSave,
						DateTime.Now)).SkipWhile((Tick tick) => tick.ID <= ransacs.vertexes.vertexList.Last().ID),
				hub.Peek().ID);
			FeedRansacsWholeQueue(hub);
			QuikTickProvider.GetInstance().Unsubscribe(param.classCode, param.secCode, hub.Enqueue);
			isUpdated = true;
		}

		/// <summary>
		/// Feeds ticks from finam hystory into ransacs session, stops when ID of tick from hystory equals to given
		/// Throws exception if there is no tick with such ID
		/// </summary>
		/// <param name="startDate"></param>
		private void FeedRansacsWithTicksUpToID(IEnumerable<Tick> ticksHystory, long stopID)
		{
			ransacs.monkeyNFilter.ReturnToLastReturned();
			foreach (Tick tick in ticksHystory)
			{
				if (tick.ID >= stopID) 
					return;
				this.ransacs.OnNewTick(tick);
			}
			throw new ArgumentException("hystoryDoesn't reach stopID");
		}
		private void FeedRansacsWithTicks(IEnumerable<Tick> ticksHystory)
		{
			ransacs.monkeyNFilter.ReturnToLastReturned();
			foreach (Tick tick in ticksHystory)
			{
				this.ransacs.OnNewTick(tick);
			}
		}
		private void FeedRansacsWholeQueue(Queue<Tick> ticks)
		{
			while(ticks.Count > 0)
			{
				ransacs.OnNewTick(ticks.Dequeue());
			}
		}

		public void SaveStandart(string path)
		{
			SaveMetadata(path);
			param.SaveStandart(path);
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
				return (DateTime.Parse((reader.ReadLine() ?? throw new Exception()).Split(';', StringSplitOptions.RemoveEmptyEntries)[1]), null);
			}
		}
	}
}
