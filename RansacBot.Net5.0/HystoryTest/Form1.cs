using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RansacBot.UI;
using RansacsRealTime;
using RansacBot.Trading;

namespace RansacBot.HystoryTest
{
	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
		}

		private void goButton_Click(object sender, EventArgs e)
		{
			Tick lastTick = new();
			List<TradeWithStopWithTick> longs = new();
			List<TradeWithStopWithTick> shorts = new();
			FileFeeder feeder = new(textBox1.Text);
			feeder.Subscribe(new("", ""), (tick) => { lastTick = tick; });
			ObservingSession session = new(new Param("SPBFUT", "RIZ1"), feeder, 100);
			session.SubscribeToProvider();

			RansacsCascade SCascade = session.AddNewRansacsCascade(SigmaType.Sigma, 1, 90);
			RansacsCascade ETCascade = session.AddNewRansacsCascade(SigmaType.ErrorThreshold, 3, 90);

			MonkeyNFinder monkeyNFinder = session.ransacs.monkeyNFilter;
			InvertedNDecider decider = new();
			HigherLowerFilterOnRansac higherLowerFilter = new(ETCascade, 2);
			MaximinStopPlacer stopPlacer = new(SCascade, 0);
			TradesHystory tradesHystory = new();
			CloserOnRansacStops closer100 = new(tradesHystory, SCascade, 0, 100);

			feeder.Subscribe(
				new("", ""),
				(tick) =>
				{
					tradesHystory.CheckForStops((decimal)tick.PRICE);
				});

			monkeyNFinder.NewExtremum += decider.OnNewExtremum;

			// plug decider to stop placer through te filter
			//decider.NewTrade += higherLowerFilter.OnNewTrade;
			//higherLowerFilter.NewTrade += stopPlacer.OnNewTrade;

			//plug decider to stop placer without filters
			decider.NewTrade += stopPlacer.OnNewTrade;

			stopPlacer.NewTradeWithStop += (tradeWithStop) =>
			{
				(tradeWithStop.direction == TradeDirection.buy ? longs : shorts).Add(new(tradeWithStop, lastTick));
			};
			stopPlacer.NewTradeWithStop += tradesHystory.OnNewTradeWithStop;
			StreamWriter dealsWriter = new(textBox2.Text + @"\deals.txt");

			tradesHystory.ExecutedLongStop += (price) =>
			{
				int tradeIndex = longs.FindIndex((trade) => (decimal)trade.trade.stop.price == price);
				TradeWithStopWithTick trade = longs[tradeIndex];
				longs.RemoveAt(tradeIndex);
				dealsWriter.WriteLine(
					"B" + ";" +
					(trade.trade.price - trade.trade.stop.price).ToString() + ";" +
					trade.tick.ID.ToString() + ";" +
					lastTick.ID.ToString() + ";" +
					trade.trade.price.ToString() + ";" +
					trade.trade.stop.price.ToString() + ";" +
					"1"
					);
			};
			tradesHystory.ExecutedShortStop += (price) =>
			{
				int tradeIndex = shorts.FindIndex((trade) => (decimal)trade.trade.stop.price == price);
				TradeWithStopWithTick trade = shorts[tradeIndex];
				shorts.RemoveAt(tradeIndex);
				dealsWriter.WriteLine(
					"S" + ";" +
					(trade.trade.stop.price - trade.trade.price).ToString() + ";" +
					trade.tick.ID.ToString() + ";" +
					lastTick.ID.ToString() + ";" +
					trade.trade.price.ToString() + ";" +
					trade.trade.stop.price.ToString() + ";" +
					"1"
					);
			};
			tradesHystory.KilledLongStop += (price) =>
			{
				int tradeIndex = longs.FindIndex((trade) => (decimal)trade.trade.stop.price == price);
				TradeWithStopWithTick trade = longs[tradeIndex];
				longs.RemoveAt(tradeIndex);
				dealsWriter.WriteLine(
					"B" + ";" +
					(trade.trade.price - trade.trade.stop.price).ToString() + ";" +
					trade.tick.ID.ToString() + ";" +
					lastTick.ID.ToString() + ";" +
					trade.trade.price.ToString() + ";" +
					lastTick.PRICE.ToString() + ";" +
					"1"
					);
			};
			tradesHystory.KilledShortStop += (price) =>
			{
				int tradeIndex = shorts.FindIndex((trade) => (decimal)trade.trade.stop.price == price);
				TradeWithStopWithTick trade = shorts[tradeIndex];
				shorts.RemoveAt(tradeIndex);
				dealsWriter.WriteLine(
					"S" + ";" +
					(trade.trade.stop.price - trade.trade.price).ToString() + ";" +
					trade.tick.ID.ToString() + ";" +
					lastTick.ID.ToString() + ";" +
					trade.trade.price.ToString() + ";" +
					lastTick.PRICE.ToString() + ";" +
					"1"
					);
			};

			progressBar1.Value = 0;
			feeder.FeedAllStandart(AddOneToProgress);
			PrintFeeding();
			session.SaveStandart(textBox2.Text);
			//SCascade.SaveStandart(textBox2.Text);
			PrintSaving();
			dealsWriter.Dispose();
		}

		void AddOneToProgress() 
		{
			//Task.Run(() => progressBar1.Invoke(new Action(() => progressBar1.Value++)));
			//progressBar1.Invoke(new Action(() => progressBar1.Value++));
			progressBar1.Value++;
		}
		void PrintFeeding()
		{
			//label3.Invoke(new Action(() => { label3.Text = "Feeding Completed!"; }));
			label3.Text = "Feeding Completed!";
		}
		void PrintSaving()
		{
			//label4.Invoke(new Action(() => { label4.Text = "Saving Completed!"; }));
			label4.Text = "Saving Completed!";
		}

		struct TradeWithStopWithTick
		{
			public readonly TradeWithStop trade;
			public readonly Tick tick;
			public TradeWithStopWithTick(TradeWithStop tradeWithStop, Tick tick)
			{
				this.trade = tradeWithStop;
				this.tick = tick;
			}
		}

		class FileFeeder : ITickByInstrumentProvider
		{
			public IEnumerable<Tick> ticks;//used for feeding
			private event TickHandler NewTick;

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
					if(count == 10000)
					{
						count = 0;
						action.Invoke();
					}
					count++;
				}
			}

			public void Subscribe(Param instrument, TickHandler handler)
			{
				NewTick += handler;
			}
			public void Unsubscribe(Param instrument, TickHandler handler)
			{
				NewTick -= handler;
			}
		}

		private void MorphData_Click(object sender, EventArgs e)
		{
			using StreamReader streamReader = new(textBox1.Text);
			using StreamWriter streamWriter = new(textBox2.Text + @"\2.txt");
			string[] readedTick;
			string dateTime;
			progressBar1.Value = 0;
			int count = 0;
			while (!streamReader.EndOfStream)
			{
				readedTick = streamReader.ReadLine().Split(';', StringSplitOptions.RemoveEmptyEntries);
				dateTime = new DateTime(
					Convert.ToInt32(readedTick[0].Substring(0, 4)),
					Convert.ToInt32(readedTick[0].Substring(4, 2)),
					Convert.ToInt32(readedTick[0].Substring(6, 2)),
					Convert.ToInt32(readedTick[1].Substring(0, 2)),
					Convert.ToInt32(readedTick[1].Substring(2, 2)),
					Convert.ToInt32(readedTick[1].Substring(4, 2))
					).Ticks.ToString();
				streamWriter.WriteLine(readedTick[4] + ',' + dateTime.ToString().Substring(0, dateTime.Length - 7) + ',' + ((int)Convert.ToDecimal(readedTick[2].Substring(0, readedTick[2].IndexOf('.')))).ToString());
				if (count == 10000)
				{
					count = 0;
					progressBar1.Value++;
				}
				count++;
			}
		}
	}
}
