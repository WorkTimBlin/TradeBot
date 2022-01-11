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

namespace RansacBot
{
	
	public partial class FormRnsacsBuildingPreview : Form
	{
		private bool stopRequired = false;
		private bool isRunning = false;
		RansacsOxyPrinter ransacsPrinter;
		public FormRnsacsBuildingPreview()
		{
			InitializeComponent();
			sigmaType.Items.Add(SigmaType.СonfidenceInterval);
			sigmaType.Items.Add(SigmaType.ErrorThreshold);
			sigmaType.Items.Add(SigmaType.Sigma);
			sigmaType.Items.Add(SigmaType.SigmaInliers);
			sigmaType.SelectedIndex = 0;
		}

		private void InitialiseTestPlotOneByOneInTime()
		{
			FileFeeder fileFeeder = new();
			ObservingSession session = new(new Instrument("RIZ1", "SPBFUT", "", "", ""), fileFeeder, 100);
			session.AddNewRansacsCascade((SigmaType)sigmaType.SelectedItem);
			session.SubscribeToProvider();
			ransacsPrinter = new RansacsOxyPrinterDemo(0, session.ransacsCascades[0], firstOnly.Checked);
			plotView1.Model = ransacsPrinter.plotModel;
			//fileFeeder.FeedAllStandart();
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
					fileFeeder.FeedOneTick(i);
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
			ObservingSession session = new(new Instrument("RIZ1", "SPBFUT", "", "", ""), fileFeeder, 100);
			session.SubscribeToProvider();
			session.AddNewRansacsCascade(RansacsRealTime.SigmaType.ErrorThreshold);
			ransacsPrinter = new RansacsOxyPrinterDemo(0, session.ransacsCascades[0], firstOnly.Checked);
			plotView1.Model = ransacsPrinter.plotModel;
			fileFeeder.FeedAllStandart();
			//session.SaveStandart("");
		}

		class FileFeeder : ITickByInstrumentProvider
		{
			public TicksLazyParser ticks = new(
					File.ReadAllText(@"C:\Users\Home\Desktop\Reps\RansacBot\BotTests\bin\Debug\net5.0-windows\TestsProperties\FolderForTests" + @"\1.txt").
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
			if(isRunning) stopRequired = true;
		}

		private void LockSigmaType()
		{
			sigmaType.Enabled = false;
		}
		private void UnlockSigmaType()
		{
			sigmaType.Enabled = true;
		}
	}
}
