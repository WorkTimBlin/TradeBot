﻿using RansacBot.Trading;
using RansacBot.UI.Components;
using RansacsRealTime;
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

namespace RansacBot.HystoryTest
{
	public partial class FlexibleHystoryRunForm : Form
	{
		public FlexibleHystoryRunForm()
		{
			InitializeComponent();
		}

		private void chooseHystoryFilePathButton_Click(object sender, EventArgs e)
		{
			inputFileDialog.ShowDialog();
			hystoryTicksFilePath.Text = inputFileDialog.InitialDirectory + inputFileDialog.FileName;
		}

		private Dictionary<SigmaType, int> GetEnoughRansacsCascades()
		{
			List<RansacLevelUsageControl> usedRansacLevels = new()
			{
				stopPlacerRansacLevelUsageControl,
				higherLowerFilterRansacLevelUsageControl,
				closingRansacLevelUsageControl
			};
			Dictionary<SigmaType, int> levels = new();
			foreach(RansacLevelUsageControl control in usedRansacLevels)
			{
				if (levels.ContainsKey(control.SigmaType))
				{
					if(control.Level > levels[control.SigmaType])
					{
						levels[control.SigmaType] = control.Level;
					}
				}
				else
				{
					levels.Add(control.SigmaType, control.Level);
				}
			}
			return levels;
		}

		private void chooseOutputDirectoryButton_Click(object sender, EventArgs e)
		{
			outputFolderBrowserDialog.ShowDialog();
			outputDirectoryTextBox.Text = outputFolderBrowserDialog.SelectedPath;
		}

		private void runButton_Click(object sender, EventArgs e)
		{
			Tick lastTick = new();

			FileFeeder fileFeeder = new(hystoryTicksFilePath.Text);
			fileFeeder.Subscribe(new("", ""), (Tick tick) => { lastTick = tick; });
			HystoryInfra hystoryInfra = HystoryInfra.Instance;
			fileFeeder.Subscribe(new("", ""), hystoryInfra.OnNewTick);
			HystoryStops hystoryStops = new();
			hystoryInfra.NewTick += hystoryStops.OnNewTick;
			ThroughProvider<Tick> provider = new();
			hystoryStops.NewTick += provider.OnNewN;
			ObservingSession session = new(new Param("", ""), provider, 100);
			Dictionary<SigmaType, RansacsCascade> cascades = new();
			foreach(KeyValuePair<SigmaType, int> i in GetEnoughRansacsCascades())
			{
				cascades.Add(i.Key, session.AddNewRansacsCascade(i.Key, i.Value, 90));
			}
			InvertedNDecider decider = new();
			session.ransacs.monkeyNFilter.NewExtremum += decider.OnNewExtremum;
			CloserOnRansacStops closer = new(
				hystoryStops, 
				cascades[closingRansacLevelUsageControl.SigmaType], 
				closingRansacLevelUsageControl.Level, 
				90);
			HigherLowerFilterOnRansac filter = new(
				cascades[higherLowerFilterRansacLevelUsageControl.SigmaType],
				higherLowerFilterRansacLevelUsageControl.Level);
			decider.NewTrade += filter.OnNewTrade;
			MaximinStopPlacer stopPlacer = new(
				cascades[stopPlacerRansacLevelUsageControl.SigmaType],
				stopPlacerRansacLevelUsageControl.Level);
			filter.NewTrade += stopPlacer.OnNewTrade;
			OneAtATimeHystoryCheckpoint checkpoint = new();
			stopPlacer.NewTradeWithStop += checkpoint.OnNewTradeWithStop;
			checkpoint.NewTradeWithStop += hystoryStops.OnNewTradeWithStop;
			session.SubscribeToProvider();



			List<TradeWithStopWithTick> longs = new();
			List<TradeWithStopWithTick> shorts = new();

			checkpoint.NewTradeWithStop += (tradeWithStop) =>
			{
				(tradeWithStop.direction == TradeDirection.buy ? longs : shorts).Add(new(tradeWithStop, lastTick));
			};
			checkpoint.NewTradeWithStop += hystoryStops.OnNewTradeWithStop;
			StreamWriter dealsWriter = new(outputDirectoryTextBox.Text + @"\deals.txt");

			hystoryStops.ExecutedLongStop += (price) =>
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
			hystoryStops.ExecutedShortStop += (price) =>
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
			hystoryStops.KilledLongStop += (price) =>
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
			hystoryStops.KilledShortStop += (price) =>
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
			Task.Run(() =>
			{
				fileFeeder.FeedAllStandart(AddOneToProgress);
				dealsWriter.Dispose();
			});
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

		private void AddOneToProgress()
		{
			progressBar1.Invoke((Action)(() => { progressBar1.Value++; }));
		}

		class OneAtATimeHystoryCheckpoint : AbstractOneAtATimeCheckpoint<TradeOrder>
		{
			protected override AbstractOrderEnsurer<TradeOrder> GetNewOrderEnsurer(Trade trade)
			{
				return new OrderOnHystoryEnsurer(new(trade));
			}
		}

		class ThroughProvider<T> : IProviderByParam<T>
		{
			public event Action<T> NewT;
			public void Subscribe(Param param, Action<T> handler)
			{
				NewT += handler;
			}

			public void Unsubscribe(Param param, Action<T> handler)
			{
				NewT -= handler;
			}
			public void OnNewN(T t)
			{
				NewT?.Invoke(t);
			}
		}
	}
}
