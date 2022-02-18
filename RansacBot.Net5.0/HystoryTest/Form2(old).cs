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
	public partial class Form2 : Form
	{
		public Form2()
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
			RansacsCascade SICascade = session.AddNewRansacsCascade(SigmaType.SigmaInliers, 4, 90);
			RansacsCascade CICascade = session.AddNewRansacsCascade(
				SigmaType.СonfidenceInterval, 1, 90
				);
			MonkeyNFinder monkeyNFinder = session.ransacs.monkeyNFilter;
			HigherLowerFilter higherLowerFilter = new();
			InvertedNDecider decider = new();
			RansacDirectionExtremumFilter directionFilter = new(CICascade, 0);
			MaximinStopPlacer stopPlacer = new(SICascade, 3);
			TradesHystory tradesHystory = new();
			CloserOnRansacStops closer50 = new(tradesHystory, SICascade, 1, 50);
			CloserOnRansacStops closer100 = new(tradesHystory, CICascade, 0, 100);

			feeder.Subscribe(new("", ""), 
				(tick) =>
					{
						tradesHystory.CheckForStops((decimal)tick.PRICE);
					});

			monkeyNFinder.NewExtremum += higherLowerFilter.OnNewExtremum;
			higherLowerFilter.NewExtremum += directionFilter.OnNewExtremum;
			directionFilter.NewExtremum += decider.OnNewExtremum;
			decider.NewTrade += stopPlacer.OnNewTrade;

			stopPlacer.NewTradeWithStop += (tradeWithStop) =>
			{
				Action<TradeWithStopWithTick> act =
					(tradeWithStop.direction == TradeDirection.buy ? longs.Add : shorts.Add);
				act(new(tradeWithStop, lastTick));
			};
			stopPlacer.NewTradeWithStop += tradesHystory.OnNewTradeWithStop;
			StreamWriter dealsWriter = new(textBox2.Text + @"\deals.txt", true);
			StreamWriter CI2Writer = new(textBox2.Text + @"\ransacs for dir filter.csv");

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

			string StringFromRansac(Ransac ransac) => ransac.firstTickIndex.ToString() + ';'
					+ ransac.firstBuildTickIndex.ToString() + ';'
					+ ransac.LastRebuildTickIndex.ToString() + ';'
					+ (ransac.EndTickIndex - 1).ToString() + ';' +
					((decimal)ransac.Slope).ToString() + ';' +
					((decimal)ransac.Intercept).ToString() + ';' +
					((decimal)ransac.Sigma).ToString() + ';' +
					((decimal)ransac.ErrorTreshold).ToString();
			Ransac currentRansac = new(0, 0, 0, 0, 0, 0, 0, 0);
			CICascade.NewRansac += (ransac, level) =>
			{
				if (level != 0) return;
				currentRansac = new(
					ransac.firstTickIndex,
					ransac.firstBuildTickIndex,
					ransac.LastRebuildTickIndex,
					ransac.Length,
					ransac.Slope,
					ransac.Intercept,
					ransac.Sigma,
					ransac.ErrorTreshold);
			};
			CICascade.RebuildRansac += (ransac, level) =>
			{
				if (level != 0) return;
				currentRansac = new(
					currentRansac.firstTickIndex,
					currentRansac.firstBuildTickIndex,
					currentRansac.LastRebuildTickIndex,
					ransac.EndTickIndex - currentRansac.firstTickIndex,
					currentRansac.Slope,
					currentRansac.Intercept,
					currentRansac.Sigma,
					currentRansac.ErrorTreshold);
				CI2Writer.WriteLine(StringFromRansac(currentRansac));
				currentRansac = new(currentRansac.LastTickIndex,
					currentRansac.LastTickIndex,
					ransac.LastRebuildTickIndex,
					0,
					ransac.Slope,
					ransac.Intercept,
					ransac.Sigma,
					ransac.ErrorTreshold
					);
			};
			feeder.FeedAllStandart(AddOneToProgress);
			PrintFeeding();
			SICascade.SaveStandart(textBox2.Text);
			CICascade.SaveStandart(textBox2.Text);
			PrintSaving();
		}

		void AddOneToProgress() 
		{
			progressBar1.Invoke(new Action(() => progressBar1.Value++));
		}
		void PrintFeeding()
		{
			label3.Invoke(new Action(() => { label3.Text = "Feeding Completed!"; }));
		}
		void PrintSaving()
		{
			label4.Invoke(new Action(() => { label4.Text = "Saving Completed!"; }));
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
				StreamReader stream = new(filePath);
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
}
