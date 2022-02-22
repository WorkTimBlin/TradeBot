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
using RansacBot.QuikRelated;
using OxyPlot.Series;
using OxyPlot;
using QuikSharp;
using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;

namespace RansacBot
{
	public partial class FormBotPreview : Form
	{
		private bool stopRequired = false;
		private bool isRunning = false;
		RansacsOxyPrinterWithTrades stopPrinter;
		RansacsOxyPrinterWithTrades filterPrinter;
		public FormBotPreview()
		{
			InitializeComponent();
		}

		private (MaximinStopPlacer stopPlacer, RansacsCascade filterCascade, RansacsCascade stopCascade) SetupTradeFilters(ObservingSession session)
		{
			RansacsCascade filterCascade = session.AddNewRansacsCascade(SigmaType.СonfidenceInterval, 1, 90);
			RansacsCascade stopCascade = session.AddNewRansacsCascade(SigmaType.SigmaInliers, 4, 90);

			HigherLowerFilter higherLowerFilter = new();
			session.ransacs.monkeyNFilter.NewExtremum += higherLowerFilter.OnNewExtremum;

			InvertedNDecider invertedNDecider = new();
			higherLowerFilter.NewExtremum += invertedNDecider.OnNewExtremum;

			RansacDirectionTradeFilter ransacDirectionFilter = new(filterCascade, 0);
			invertedNDecider.NewTrade += ransacDirectionFilter.OnNewTrade;

			MaximinStopPlacer maximinStopPlacer = new(stopCascade, 2);
			ransacDirectionFilter.NewTrade += maximinStopPlacer.OnNewTrade;
			return (maximinStopPlacer, filterCascade, stopCascade);
		}

		private ObservingSession InitAndSetupSession(IProviderByParam<Tick> provider, ITradesHystory tradesHystory, ITradeWithStopFilter tradeWithStopProcessor)
		{
			ObservingSession session = new(new Param("SPBFUT", "RIH2"), provider, (int)numericUpDown_NSetter.Value);

			var setup = SetupTradeFilters(session);
			RansacsCascade stopCascade = setup.stopCascade;
			RansacsCascade filterCascade = setup.filterCascade;
			MaximinStopPlacer maximinStopPlacer = setup.stopPlacer;
			
			stopPrinter = new RansacsOxyPrinterWithTradesDemo(2, stopCascade, firstOnly.Checked);
			plotView1.Model = stopPrinter.plotModel;

			filterPrinter = new RansacsOxyPrinterWithTradesDemo(1, stopCascade, firstOnly.Checked);
			plotView2.Model = filterPrinter.plotModel;


			//maximinStopPlacer.NewTradeWithStop += stopPrinter.OnNewTradeWithStop;
			session.ransacs.monkeyNFilter.NewExtremum += stopPrinter.OnNewExtremum;

			OldCloserOnRansacStops closerOnRansacStops = new(tradesHystory, stopCascade, 1, 50);
			OldCloserOnRansacStops closerOnRansacStops1 = new(tradesHystory, filterCascade, 0, 100);

			maximinStopPlacer.NewTradeWithStop += tradeWithStopProcessor.OnNewTradeWithStop;
			tradeWithStopProcessor.NewTradeWithStop += stopPrinter.OnNewTradeWithStop;

			tradesHystory.KilledLongStop += stopPrinter.OnClosePos;
			tradesHystory.KilledShortStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedLongStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedShortStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedLongStop += (price, exPrice) => { Console.WriteLine(DateTime.Now.ToString() + " " + "long stops executed"); };
			tradesHystory.ExecutedShortStop += (price, exPrice) => { Console.WriteLine(DateTime.Now.ToString() + " " + "short stops executed"); };

			session.SubscribeToProvider();
			return session;
		}
		
		private ObservingSession InitAndSetupSession_2_0(IProviderByParam<Tick> provider, ITradesHystory tradesHystory, ITradeWithStopFilter tradeWithStopEnsurer)
		{
			ObservingSession session = new(new Param("SPBFUT", "RIH2"), provider, (int)numericUpDown_NSetter.Value);

			RansacsCascade SCascade = session.AddNewRansacsCascade(SigmaType.Sigma, 1, 90);
			RansacsCascade ETCascade = session.AddNewRansacsCascade(SigmaType.ErrorThreshold, 3, 90);

			stopPrinter = new RansacsOxyPrinterWithTradesDemo(0, SCascade, firstOnly.Checked);
			plotView1.Model = stopPrinter.plotModel;

			filterPrinter = new RansacsOxyPrinterWithTradesDemo(2, ETCascade, firstOnly.Checked);
			plotView2.Model = filterPrinter.plotModel;

			session.ransacs.monkeyNFilter.NewExtremum += stopPrinter.OnNewExtremum;

			InvertedNDecider invertedNDecider = new();
			HigherLowerFilterOnRansac higherLowerFilter = new(ETCascade, 2);
			MaximinStopPlacer maximinStopPlacer = new(SCascade, 0);

			OldCloserOnRansacStops closerOnRansacStops = new(tradesHystory, SCascade, 0, 100);

			session.ransacs.monkeyNFilter.NewExtremum += invertedNDecider.OnNewExtremum;

			//invertedNDecider.NewTrade += higherLowerFilter.OnNewTrade;
			//higherLowerFilter.NewTrade += maximinStopPlacer.OnNewTrade;

			invertedNDecider.NewTrade += maximinStopPlacer.OnNewTrade;

			maximinStopPlacer.NewTradeWithStop += tradeWithStopEnsurer.OnNewTradeWithStop;
			tradeWithStopEnsurer.NewTradeWithStop += tradesHystory.OnNewTradeWithStop;
			tradeWithStopEnsurer.NewTradeWithStop += stopPrinter.OnNewTradeWithStop;

			tradesHystory.KilledLongStop += stopPrinter.OnClosePos;
			tradesHystory.KilledShortStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedLongStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedShortStop += stopPrinter.OnClosePos;
			tradesHystory.ExecutedLongStop += (price, exPrice) => { Console.WriteLine(DateTime.Now.ToString() + " " + "long stops executed"); };
			tradesHystory.ExecutedShortStop += (price, exPrice) => { Console.WriteLine(DateTime.Now.ToString() + " " + "short stops executed"); };

			session.SubscribeToProvider();
			return session;
		}

		private void InitialiseTestPlotOneByOneInTime()
		{
			FileFeeder fileFeeder = new();
			TradesHystory tradesHystory = new();

			InitAndSetupSession_2_0(fileFeeder, tradesHystory, tradesHystory);
			//ransacs.vertexes.NewVertex += (Tick tick, VertexType vertexType) => { tradesHystory.CheckForStops((decimal)tick.PRICE); };

			tradesHystory.NewTradeWithStop -= tradesHystory.OnNewTradeWithStop;

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
		}

		private void InitialiseTestPlotAllAtOnce()
		{
			FileFeeder fileFeeder = new();
			TradesHystory tradesHystory = new();
			InitAndSetupSession_2_0(fileFeeder, tradesHystory, tradesHystory).
				ransacs.vertexes.NewVertex += (Tick tick, VertexType vertexType) => { tradesHystory.CheckForStops((decimal)tick.PRICE); };

			tradesHystory.NewTradeWithStop -= tradesHystory.OnNewTradeWithStop;

			fileFeeder.FeedAllStandart();
		}

		private StopStorageClassic stopStorage;
		private void BuildPlotFromQuickTicks()
		{
			QuikTickProvider quikTickProvider = QuikTickProvider.GetInstance();

			LockNSetter();
			//QuikTradeConnector quikTradeConnector = new(new Param("SPBFUT", "RIH2"), "SPBFUT005gx");
			TradeParams tradeParams = new("SPBFUT", "RIH2", "SPBFUT0067Y", "50290");
			stopStorage = new(tradeParams);
			OneOrderAtATimeCheckpoint ensurer = new(tradeParams);
			ObservingSession session = InitAndSetupSession_2_0(quikTickProvider, stopStorage, ensurer);

			Quik quik = QuikContainer.Quik;
			//listBox1.Items.Add(quik.Trading.GetParamEx("SPBFUT", "RIH2", ParamNames.HIGH).Result.ParamValue.ToString());
			//listBox1.Items.Add(quik.StopOrders.GetStopOrders().Result[^1].TransId);

			void OnStopClick(object sender, EventArgs e)
			{
				this.stop.Click -= OnStopClick;
				stopStorage.ClosePercentOfLongs(100);
				stopStorage.ClosePercentOfShorts(100);
				session.UnsubscribeOfProvider();
				stopRequired = false;
				UnlockNSetter();
			}
			this.stop.Click += OnStopClick;
			timer1.Start();
		}


		class FileFeeder : IProviderByParam<Tick>
		{
			static string standartLocation = Directory.GetCurrentDirectory().
#if DEBUG
						Replace(@"\RansacBot.Net5.0\bin\Debug\net5.0-windows", @"\BotTests\bin\Debug\net5.0-windows\TestsProperties\FolderForTests" + @"\1.txt");
#endif
#if RELEASE
						Replace(@"\RansacBot.Net5.0\bin\Release\net5.0-windows", @"\BotTests\bin\Debug\net5.0-windows\TestsProperties\FolderForTests" + @"\1.txt"));
#endif
			static string fullRiz1Loc = @"C:\Users\ir2\Desktop\1.txt";
			public TicksLazyParser ticks = new(
					File.ReadAllText(fullRiz1Loc).
					Split("\r\n", StringSplitOptions.RemoveEmptyEntries));//used for feeding
			private event Action<Tick> NewTick;

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

			public void Subscribe(Param instrument, Action<Tick> handler)
			{
				NewTick += handler;
			}
			public void Unsubscribe(Param instrument, Action<Tick> handler)
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

		private void timer1_Tick(object sender, EventArgs e)
		{
			listBox1.Items.Clear();
			listBox1.Items.AddRange(stopStorage.GetLongs().ToArray());
			listBox1.Items.Add("");
			listBox1.Items.AddRange(stopStorage.GetShorts().ToArray());
		}
	}
}
