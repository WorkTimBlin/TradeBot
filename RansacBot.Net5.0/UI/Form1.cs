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
	public partial class FormMain : Form
	{
		RansacsOxyPrinter ransacsPrinter;
		public FormMain()
		{
			InitializeComponent();
		}

		private void InitialiseTestPlot()
		{
			FileFeeder fileFeeder = new();
			ObservingSession session = new(new Instrument("RIZ1", "SPBFUT", "", "", ""), fileFeeder, 100);
			session.AddNewRansacsCascade(SigmaType.ErrorThreshold);
			ransacsPrinter = new(1, session.ransacsCascades[0]);
			plotView1.Model = ransacsPrinter.plotModel;
			//fileFeeder.FeedAllStandart();
			Task.Run(() =>
			{
				for (int i = 0; i < fileFeeder.ticks.Count; i++)
				{
					fileFeeder.FeedOneTick(i);
					Task.Delay(1).Wait();
				}
			});
			//session.SaveStandart("");
		}

		class FileFeeder : ITickByInstrumentProvider
		{
			public TicksLazyParser ticks = new(
					File.ReadAllText(@"D:\work\programming\MMRC\BotTests\bin\Debug\net5.0-windows\TestsProperties\FolderForTests" + @"\1.txt").
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

		private void OnShowDemo(object sender, EventArgs e)
		{
			InitialiseTestPlot();
		}
	}
}
