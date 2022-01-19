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
using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;

namespace RansacBot
{
	public partial class FormRansacsWithTradesBuildingPreview : Form
	{
		private bool stopRequired = false;
		private bool isRunning = false;
		RansacsOxyPrinterWithTrades stopPrinter;
		RansacsOxyPrinterWithTrades filterPrinter;
		public FormRansacsWithTradesBuildingPreview()
		{
			InitializeComponent();
			sigmaType.Items.Add(SigmaType.СonfidenceInterval);
			sigmaType.Items.Add(SigmaType.ErrorThreshold);
			sigmaType.Items.Add(SigmaType.Sigma);
			sigmaType.Items.Add(SigmaType.SigmaInliers);
			sigmaType.SelectedIndex = 0;
		}

		private (MaximinStopPlacer stopPlacer, RansacsCascade filterCascade, RansacsCascade stopCascade) SetupTradeFilters(ObservingSession session)
		{
			RansacsCascade filterCascade = session.AddNewRansacsCascade(SigmaType.СonfidenceInterval, 1, 90);
			RansacsCascade stopCascade = session.AddNewRansacsCascade(SigmaType.SigmaInliers, 4, 90);

			HigherLowerFilter higherLowerFilter = new();
			session.ransacs.monkeyNFilter.NewExtremum += higherLowerFilter.OnNewExtremum;

			InvertedNDecider invertedNDecider = new();
			higherLowerFilter.NewExtremum += invertedNDecider.OnNewExtremum;

			RansacDirectionFilter ransacDirectionFilter = new(filterCascade, 0);
			invertedNDecider.NewTrade += ransacDirectionFilter.OnNewTrade;

			MaximinStopPlacer maximinStopPlacer = new(stopCascade, 3);
			ransacDirectionFilter.NewTrade += maximinStopPlacer.OnNewTrade;
			return (maximinStopPlacer, filterCascade, stopCascade);
		}

		private ObservingSession InitAndSetupSession(ITickByInstrumentProvider provider, ITradesHystory tradesHystory)
		{
			ObservingSession session = new(new Param("SPBFUT", "RIH2"), provider, (int)numericUpDown_NSetter.Value);

			var setup = SetupTradeFilters(session);
			RansacsCascade stopCascade = setup.stopCascade;
			RansacsCascade filterCascade = setup.filterCascade;
			MaximinStopPlacer maximinStopPlacer = setup.stopPlacer;
			
			stopPrinter = new RansacsOxyPrinterWithTradesDemo(3, stopCascade, firstOnly.Checked);
			plotView1.Model = stopPrinter.plotModel;

			filterPrinter = new RansacsOxyPrinterWithTradesDemo(1, stopCascade, firstOnly.Checked);
			plotView2.Model = filterPrinter.plotModel;


			//maximinStopPlacer.NewTradeWithStop += stopPrinter.OnNewTradeWithStop;
			session.ransacs.monkeyNFilter.NewExtremum += stopPrinter.OnNewExtremum;

			CloserOnRansacStops closerOnRansacStops = new(tradesHystory, stopCascade, 1, 50);
			CloserOnRansacStops closerOnRansacStops1 = new(tradesHystory, filterCascade, 0, 100);

			maximinStopPlacer.NewTradeWithStop += tradesHystory.OnNewTradeWithStop;
			tradesHystory.NewTradeWithStop += stopPrinter.OnNewTradeWithStop;

			tradesHystory.KilledLongStop += stopPrinter.OnClosePos;
			tradesHystory.KilledShortStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedLongStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedShortStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedLongStop += (decimal list) => { Console.WriteLine("long stops executed"); };
			tradesHystory.ExecutedShortStop += (decimal list) => { Console.WriteLine("short stops executed"); };

			session.SubscribeToProvider();
			return session;
		}
		
		private void InitialiseTestPlotOneByOneInTime()
		{
			FileFeeder fileFeeder = new();
			QuikTradeConnector tradesHystory = new(new("SPBFUT", "RIH2"), "SPBFUT005gx");
			InitAndSetupSession(fileFeeder, tradesHystory);
				//ransacs.vertexes.NewVertex += (Tick tick, VertexType vertexType) => { tradesHystory.CheckForStops((decimal)tick.PRICE); };

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
		}

		private void InitialiseTestPlotAllAtOnce()
		{
			FileFeeder fileFeeder = new();
			TradesHystory tradesHystory = new();
			InitAndSetupSession(fileFeeder, tradesHystory).
				ransacs.vertexes.NewVertex += (Tick tick, VertexType vertexType) => { tradesHystory.CheckForStops((decimal)tick.PRICE); };

			fileFeeder.FeedAllStandart();
		}


		private void BuildPlotFromQuickTicks()
		{
			QuikTickProvider quikTickProvider = QuikTickProvider.GetInstance();

			LockNSetter();
			QuikTradeConnector quikTradeConnector = new(new Param("SPBFUT", "RIH2"), "SPBFUT005gx");
			ObservingSession session = InitAndSetupSession(quikTickProvider, quikTradeConnector);

			Quik quik = QuikContainer.Quik;
			listBox1.Items.Add(quik.Trading.GetParamEx("SPBFUT", "RIH2", ParamNames.HIGH).Result.ParamValue.ToString());
			listBox1.Items.Add(quik.StopOrders.GetStopOrders().Result[^1].TransId);

			void OnStopClick(object sender, EventArgs e)
			{
				this.stop.Click -= OnStopClick;
				session.UnsubscribeOfProvider();
				stopRequired = false;
				UnlockNSetter();
			}
			this.stop.Click += OnStopClick;
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

			public void Subscribe(Param instrument, TickHandler handler)
			{
				NewTick += handler;
			}
			public void Unsubscribe(Param instrument, TickHandler handler)
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
		private void UnlockNSetter()
		{
			numericUpDown_NSetter.Enabled = true;
		}

		private void reloadQuik_Click(object sender, EventArgs e)
		{
			QuikContainer.ReloadQuik();
		}
	}
}
