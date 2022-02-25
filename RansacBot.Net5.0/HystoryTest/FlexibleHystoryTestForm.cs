using RansacBot.Trading;
using RansacBot.Trading.Hystory;
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
			ThroughProvider<Tick> provider = new();
			ObservingSession session = new(new Param("", ""), provider, 100);
			Dictionary<SigmaType, RansacsCascade> cascades = new();
			foreach(KeyValuePair<SigmaType, int> i in GetEnoughRansacsCascades())
			{
				cascades.Add(i.Key, session.AddNewRansacsCascade(i.Key, i.Value, 90));
			}
			InvertedNDecider decider = new();
			session.ransacs.monkeyNFilter.NewExtremum += decider.OnNewExtremum;
			
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
			session.SubscribeToProvider();



			List<TradeWithStopWithTick> longs = new();
			List<TradeWithStopWithTick> shorts = new();

			checkpoint.NewTradeWithStop += (tradeWithStop) =>
			{
				(tradeWithStop.direction == TradeDirection.buy ? longs : shorts).Add(new(tradeWithStop, lastTick));
			};
			StreamWriter dealsWriter = new(outputDirectoryTextBox.Text + @"\deals.txt");
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

		class OneAtATimeHystoryCheckpoint : AbstractOneAtATimeCheckpoint<HystoryOrder>
		{
			protected override AbstractOrderEnsurerWithPrice<HystoryOrder> GetNewOrderEnsurer(Trade trade)
			{
				return new OrderOnHystoryEnsurer(new(trade));
			}
		}
	}
}
