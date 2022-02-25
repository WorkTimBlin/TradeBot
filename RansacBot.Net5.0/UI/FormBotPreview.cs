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

namespace RansacBot
{
	public partial class FormBotPreview : Form
	{
		private bool stopRequired = false;
		private bool isRunning = false;
		RansacsOxyPrinterWithTrades stopPrinter;
		RansacsOxyPrinterWithTrades filterPrinter;
		IStopsContainer stopsContainer;

		TradeParams tradeParams = new("SPBFUT", "RIH2", "SPBFUT0067Y", "50290");
		S2_ET_S2_DecisionMaker decisionMaker;



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
			if (isRunning) stopRequired = true;
			timer1.Stop();
			if(decisionMaker != null)
			{
				QuikTickProvider.GetInstance().Unsubscribe(tradeParams, decisionMaker.OnNewTick);
			}
			UnlockNSetter();
			UnlockUseFilterCheckbox();
		}

		private void buttonQuickWatch_Click(object sender, EventArgs e)
		{
			LockUseFilterCheckbox();
			LockNSetter();

			decisionMaker = new(useFilterCheckbox.Checked, (int)numericUpDown_NSetter.Value);
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

			decisionMaker.vertexProvider.NewExtremum += stopPrinter.OnNewExtremum;
			tradingModule.TradeExecuted += stopPrinter.OnNewTradeWithStop;
			tradingModule.StopExecutedOnPrice += (trade, price) => stopPrinter.OnClosePos(trade.stop.price, price);
			tradingModule.TradeClosedOnPrice += (trade, price) => stopPrinter.OnClosePos(trade.stop.price, price);

			timer1.Start();
			QuikTickProvider.GetInstance().Subscribe(tradeParams, decisionMaker.OnNewTick);
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
			listBox1.Items.Clear();
			listBox1.Items.AddRange(stopsContainer.GetLongs().ToArray());
			listBox1.Items.Add("");
			listBox1.Items.AddRange(stopsContainer.GetShorts().ToArray());
		}
	}
}
