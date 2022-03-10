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
using RansacBot.HystoryTest;
using RansacBot.Assemblies;
using RansacBot.Trading.Hystory.Infrastructure;

namespace RansacBot
{
	public partial class FormBotPreview : Form
	{
		RansacsOxyPrinterWithTrades stopPrinter;
		RansacsOxyPrinterWithTrades filterPrinter;
		IStopsContainer stopsContainer;

		TradeParams tradeParams = new("SPBFUT", "RIH2", "SPBFUT0067Y", "50290");
		IDecisionProvider decisionProvider;



		public FormBotPreview()
		{
			InitializeComponent();
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
			timer1.Stop();
			if(decisionProvider != null)
			{
				QuikTickProvider.GetInstance().Unsubscribe(tradeParams, decisionProvider.OnNewTick);
			}
			UnlockNSetter();
			UnlockUseFilterCheckbox();
		}

		private void buttonQuikWatch_Click(object sender, EventArgs e)
		{
			LockUseFilterCheckbox();
			LockNSetter();

			S2_ET_S2_DecisionMaker decisionMaker = 
				new S2_ET_S2_DecisionMaker(useFilterCheckbox.Checked, (int)numericUpDown_NSetter.Value);
			decisionProvider = decisionMaker;
			
			QuikTradingModule tradingModule = 
				new(
					tradeParams, 
					decisionMaker.TradeWithStopProvider, 
					decisionMaker.ClosingProvider);

			stopPrinter = new(0, decisionMaker.SCascade);
			filterPrinter = new(2, decisionMaker.ETCascade);
			plotView1.Model = stopPrinter.plotModel;
			plotView2.Model = filterPrinter.plotModel;

			stopsContainer = tradingModule.StopsContainer;

			decisionMaker.VertexProvider.NewExtremum += stopPrinter.OnNewExtremum;
			tradingModule.TradeExecuted += stopPrinter.OnNewTradeWithStop;
			tradingModule.StopExecutedOnPrice += (trade, price) => stopPrinter.OnClosePos(trade.stop.price, price);
			tradingModule.TradeClosedOnPrice += (trade, price) => stopPrinter.OnClosePos(trade.stop.price, price);

			timer1.Start();
			QuikTickProvider.GetInstance().Subscribe(tradeParams, decisionMaker.OnNewTick);
		}

		private void showHystoryDemoButton_Click(object sender, EventArgs e)
		{
			LockUseFilterCheckbox();
			LockNSetter();

			S2_ET_S2_DecisionMaker decisionMaker =
				new S2_ET_S2_DecisionMaker(useFilterCheckbox.Checked, (double)numericUpDown_NSetter.Value);
			decisionProvider = decisionMaker;

			HystoryTradingModule tradingModule =
				new(
					decisionMaker.TradeWithStopProvider,
					decisionMaker.ClosingProvider);

			stopPrinter = new(0, decisionMaker.SCascade);
			filterPrinter = new(2, decisionMaker.ETCascade);
			plotView1.Model = stopPrinter.plotModel;
			plotView2.Model = filterPrinter.plotModel;

			stopsContainer = tradingModule.StopsContainer;

			decisionMaker.VertexProvider.NewExtremum += stopPrinter.OnNewExtremum;
			tradingModule.TradeExecuted += stopPrinter.OnNewTradeWithStop;
			tradingModule.StopExecutedOnPrice += (trade, price) => stopPrinter.OnClosePos(trade.stop.price, price);
			tradingModule.TradeClosedOnPrice += (trade, price) => stopPrinter.OnClosePos(trade.stop.price, price);


			FinishedTradesProvider finishedTradesBuilder = new();
			tradingModule.TradeExecuted += finishedTradesBuilder.OnTradeOpened;
			tradingModule.TradeClosedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;
			tradingModule.StopExecutedOnPrice += finishedTradesBuilder.OnTradeClosedOnPrice;
			
			StreamWriter writer = new(@"C:\Users\ir2\Desktop\hystoryDeals.txt");
			finishedTradesBuilder.NewTradeFinished += (finishedTrade) =>
			{
				writer.WriteLine(finishedTrade.ToString());
			};


			HystoryQuikSimulator quikSimulator = tradingModule.QuikSimulator;
			finishedTradesBuilder.NewTick += quikSimulator.OnNewTick;
			quikSimulator.NewTick += decisionMaker.OnNewTick;

			timer1.Start();
			Task.Run(() =>
			{
				ITicksParser parser = TicksParser.FinamStandart;
				foreach (Tick tick in
					File.ReadLines(@"C:\Users\ir2\Desktop\1.txt").Select(parser.ParseTick))
				{
					finishedTradesBuilder.OnNewTick(tick);
				}
			}).ContinueWith((task) =>
			{
				this.Invoke((Action)(() =>
				{
					UnlockNSetter();
					UnlockUseFilterCheckbox();
					timer1.Stop();
					UpdateStopsList();
				}));
				writer.Dispose();
			});
		}


		private void LockUseFilterCheckbox()
		{
			useFilterCheckbox.Enabled = false;
		}
		private void UnlockUseFilterCheckbox()
		{
			useFilterCheckbox.Enabled = true;
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
			UpdateStopsList();
		}

		private void UpdateStopsList()
		{
			listBox1.Items.Clear();
			listBox1.Items.AddRange(stopsContainer.GetShorts().ToArray());
			listBox1.Items.Add("");
			listBox1.Items.AddRange(stopsContainer.GetLongs().ToArray());
			listBox1.Items.Add("");
			listBox1.Items.Add("");
			listBox1.Items.AddRange(stopsContainer.GetExecuted().ToArray());
		}
	}
}
