using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RansacsRealTime;
using RansacBot.UI;
using RansacBot.Trading;
using OxyPlot.Series;
using OxyPlot;
using QuikSharp;

namespace RansacBot
{
	public partial class FormRansacsWithTradesBuildingPreview : Form
	{
		private bool stopRequired = false;
		private bool isRunning = false;
		RansacsOxyPrinterWithTrades ransacsPrinter;
		public FormRansacsWithTradesBuildingPreview()
		{
			InitializeComponent();
			sigmaType.Items.Add(SigmaType.СonfidenceInterval);
			sigmaType.Items.Add(SigmaType.ErrorThreshold);
			sigmaType.Items.Add(SigmaType.Sigma);
			sigmaType.Items.Add(SigmaType.SigmaInliers);
			sigmaType.SelectedIndex = 0;
		}
		int level = 1;
		
		private void InitialiseTestPlotOneByOneInTime()
		{
			FileFeeder fileFeeder = new();

			ObservingSession session = new(new Instrument("SPBFUT", "RIH2", "", "", ""), fileFeeder, 100);
			
			session.AddNewRansacsCascade((SigmaType)sigmaType.SelectedItem);
			session.SubscribeToProvider();
			
			RansacsCascade filterCascade = session.AddNewRansacsCascade(SigmaType.СonfidenceInterval, 1, 90);
			RansacsCascade stopCascade = session.AddNewRansacsCascade(SigmaType.SigmaInliers, 4, 90);
			
			ransacsPrinter = new RansacsOxyPrinterWithTradesDemo(level, stopCascade, firstOnly.Checked);
			plotView1.Model = ransacsPrinter.plotModel;
			//fileFeeder.FeedAllStandart();

			HigherLowerFilter higherLowerFilter = new();
			session.ransacs.monkeyNFilter.NewExtremum += higherLowerFilter.OnNewExtremum;
			
			InvertedNDecider invertedNDecider = new();
			higherLowerFilter.NewExtremum += invertedNDecider.OnNewExtremum;

			RansacDirectionFilter ransacDirectionFilter = new(filterCascade, 0);
			invertedNDecider.NewTrade += ransacDirectionFilter.OnNewTrade;

			MaximinStopPlacer maximinStopPlacer = new(stopCascade, 3);
			ransacDirectionFilter.NewTrade += maximinStopPlacer.OnNewTrade;

			maximinStopPlacer.NewTradeWithStop += ransacsPrinter.OnNewTradeWithStop;
			session.ransacs.monkeyNFilter.NewExtremum += ransacsPrinter.OnNewExtremum;

			TradesHystory tradesHystory = new();
			
			//CloserOnRansacStops closerOnRansacStops = new(tradesHystory, stopCascade, 1, 50);
			//CloserOnRansacStops closerOnRansacStops1 = new(tradesHystory, filterCascade, 0, 100);
			
			maximinStopPlacer.NewTradeWithStop += tradesHystory.OnNewTrade;

			tradesHystory.CloseLongs += ransacsPrinter.OnClosePos;
			tradesHystory.CloseShorts += ransacsPrinter.OnClosePos;
			tradesHystory.ExecuteLongStops += ransacsPrinter.OnClosePos;
			tradesHystory.ExecuteShortStops += ransacsPrinter.OnClosePos;

			session.ransacs.vertexes.NewVertex += (Tick tick, VertexType vertexType) => { tradesHystory.CheckForStops(tick.PRICE); };
			




			LockSigmaType();
			Task feeding = Task.Run(() =>
			{
				isRunning = true;
				for (int i = 0; i < fileFeeder.ticks.Count; i++)
				{
					while (pause.Checked) { }
					if (stopRequired)
					{
						stopRequired = false;
						isRunning = false;
						return;
					}
					int j = i + (int)numericUpDown_Speed.Value;
					for(; i < j && i < fileFeeder.ticks.Count; i++)
					{
						fileFeeder.FeedOneTick(i);
					}
					Task.Delay(1).Wait();
				}
				isRunning = false;
			});
			feeding.GetAwaiter().OnCompleted(UnlockSigmaType);
			//session.SaveStandart("");
		}

		private void InitialiseTestPlotAllAtOnce()
		{
			FileFeeder fileFeeder = new();
			ObservingSession session = new(new Instrument("SPBFUT", "RIH2", "", "", ""), fileFeeder, 100);
			
			session.SubscribeToProvider();
			session.AddNewRansacsCascade((SigmaType)sigmaType.SelectedItem);
			
			RansacsCascade filterCascade = session.AddNewRansacsCascade(SigmaType.СonfidenceInterval, 1, 90);
			RansacsCascade stopCascade = session.AddNewRansacsCascade(SigmaType.SigmaInliers, 4, 90);
			
			ransacsPrinter = new RansacsOxyPrinterWithTradesDemo(3, stopCascade, firstOnly.Checked);
			plotView1.Model = ransacsPrinter.plotModel;
			
			HigherLowerFilter higherLowerFilter = new();
			session.ransacs.monkeyNFilter.NewExtremum += higherLowerFilter.OnNewExtremum;
			
			InvertedNDecider invertedNDecider = new();
			higherLowerFilter.NewExtremum += invertedNDecider.OnNewExtremum;

			RansacDirectionFilter ransacDirectionFilter = new(filterCascade, 0);
			invertedNDecider.NewTrade += ransacDirectionFilter.OnNewTrade;

			MaximinStopPlacer maximinStopPlacer = new(stopCascade, 3);
			ransacDirectionFilter.NewTrade += maximinStopPlacer.OnNewTrade;

			maximinStopPlacer.NewTradeWithStop += ransacsPrinter.OnNewTradeWithStop;
			session.ransacs.monkeyNFilter.NewExtremum += ransacsPrinter.OnNewExtremum;


			fileFeeder.FeedAllStandart();
			//session.SaveStandart("");
		}


		private void BuildPlotFromQuickTicks()
		{
			QuikTickProvider _instance = QuikTickProvider.GetInstance();

			LockNSetter();
			ObservingSession session = new(new Instrument("SPBFUT", "RIH2", "", "", ""), _instance, (int)numericUpDown_NSetter.Value);

			session.AddNewRansacsCascade((SigmaType)sigmaType.SelectedItem);

			RansacsCascade filterCascade = session.AddNewRansacsCascade(SigmaType.СonfidenceInterval, 1, 90);
			RansacsCascade stopCascade = session.AddNewRansacsCascade(SigmaType.SigmaInliers, 4, 90);

			ransacsPrinter = new RansacsOxyPrinterWithTradesDemo(3, stopCascade, firstOnly.Checked);
			plotView1.Model = ransacsPrinter.plotModel;

			HigherLowerFilter higherLowerFilter = new();
			session.ransacs.monkeyNFilter.NewExtremum += higherLowerFilter.OnNewExtremum;

			InvertedNDecider invertedNDecider = new();
			higherLowerFilter.NewExtremum += invertedNDecider.OnNewExtremum;

			RansacDirectionFilter ransacDirectionFilter = new(filterCascade, 0);
			invertedNDecider.NewTrade += ransacDirectionFilter.OnNewTrade;

			MaximinStopPlacer maximinStopPlacer = new(stopCascade, 3);
			ransacDirectionFilter.NewTrade += maximinStopPlacer.OnNewTrade;

			maximinStopPlacer.NewTradeWithStop += ransacsPrinter.OnNewTradeWithStop;
			session.ransacs.monkeyNFilter.NewExtremum += ransacsPrinter.OnNewExtremum;

			session.SubscribeToProvider();
		}


		class FileFeeder : ITickByInstrumentProvider
		{
			public TicksLazyParser ticks = new(
					File.ReadAllText(Directory.GetCurrentDirectory().
#if DEBUG
						Replace(@"\RansacBot.Net5.0\bin\Debug\net5.0-windows", @"\BotTests\bin\Debug\net5.0-windows\TestsProperties\FolderForTests" + @"\1.txt")).
#endif
#if RELEASE
						Replace(@"\RansacBot.Net5.0\bin\Release\net5.0-windows", @"\BotTests\bin\Debug\net5.0-windows\TestsProperties\FolderForTests" + @"\1.txt")).
#endif
					Split("\r\n", StringSplitOptions.RemoveEmptyEntries));//used for feeding
			private event TickHandler NewTick;

			public void FeedAllStandart()
			{
				foreach (Tick tick in ticks)
				{
					NewTick.Invoke(tick);
				}
			}
			public void FeedRangeOfStandart(int startIndex, int count)
			{
				for (int i = startIndex; i < startIndex + count; i++)
				{
					NewTick.Invoke(ticks[i]);
				}
			}
			public void FeedOneTick(int index)
			{
				NewTick.Invoke(ticks[index]);
			}

			public void Subscribe(Instrument instrument, TickHandler handler)
			{
				NewTick += handler;
			}
			public void Unsubscribe(Instrument instrument, TickHandler handler)
			{
				NewTick -= handler;
			}
		}

		private void OnShowDemoProgressing(object sender, EventArgs e)
		{
			InitialiseTestPlotOneByOneInTime();
		}
		private void OnShowDemoAllAtOnce(object sender, EventArgs e)
		{
			InitialiseTestPlotAllAtOnce();
		}

		private void pause_CheckedChanged(object sender, EventArgs e)
		{
			if (pause.Checked)
			{
				pause.Text = ">";
			}
			else
			{
				pause.Text = "II";
			}
		}

		private void stop_Click(object sender, EventArgs e)
		{
			if (isRunning) stopRequired = true;
		}

		private void LockSigmaType()
		{
			sigmaType.Enabled = false;
		}
		private void UnlockSigmaType()
		{
			sigmaType.Enabled = true;
		}

		private void buttonQuickWatch_Click(object sender, EventArgs e)
		{
			BuildPlotFromQuickTicks();
		}

		private void LockNSetter()
		{
			numericUpDown_NSetter.Enabled = false;
		}
	}
}
