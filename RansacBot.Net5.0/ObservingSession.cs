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
			Connector.Subscribe(instrument.classCode, instrument.securityCode, this.ransacs.OnNewTick);
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
		}

		public void AddNewRansacsCascade(TypeSigma typeSigma, double percentile = 90)
		{
			new RansacsCascade(this.ransacs.vertexes, typeSigma, percentile);
		}

		//TODO:complete
		public void UpdateUpToNow()
		{
			Queue<Tick> hub = new();
			Connector.Subscribe(instrument.classCode, instrument.securityCode, hub.Enqueue);
			DateTime targetDateTime = DateTime.Now + new TimeSpan(0, 2, 0);
			WaitWhile(() => { return DateTime.Now >= targetDateTime; });
		}

		/// <summary>
		/// Feeds ticks from finam hystory into ransacs session, stops when ID of tick from hystory equals to given
		/// Throws exception if there is no tick with such ID
		/// </summary>
		/// <param name="startDate"></param>
		public void FeedTheGapWithTicksUpToID(IEnumerable<Tick> ticksHystory, long stopID)
		{
			foreach(Tick tick in ticksHystory)
			{
				if (tick.ID == stopID) return;
				this.ransacs.OnNewTick(tick);
			}
			throw new ArgumentException("hystoryDoesn't reach stopID");
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
