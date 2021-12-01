using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp;
using RansacRealTime;

namespace RansacBot.Net5._0.Tests
{
	static public class SaveLoadTests
	{
		static public void TestVertexesSaveAll(TimeSpan timeToWaitBeforeSave, TimeSpan timeToWaitBeforeLoad, TimeSpan timeToWaitBeforeEnd)
		{
			//string classCode = "SPBFUT";
			//string secCode = "BRS1";
			string classCode = "SPBFUT";
			string secCode = "RiZ1";
			Vertexes vertexes = new();
			RansacsHystory hystory = new(vertexes, TypeSigma.СonfidenceInterval);
			MonkeyNFilter MNfilter = new(0.05f);
			Connector.Subscribe(classCode, secCode, MNfilter.OnNewTick);
			MNfilter.NewVertex += vertexes.OnNewVertex;
			ProgressCounter counter = new((int)timeToWaitBeforeSave.TotalSeconds);
			DateTime start = DateTime.Now;
			DateTime now = DateTime.Now;
			//Console.WriteLine("");
			while (now - start < timeToWaitBeforeSave)
			{
				now = DateTime.Now;
			}
			MNfilter.NewVertex -= vertexes.OnNewVertex;
			vertexes.SaveStandart("", true);
			start = DateTime.Now;
			now = DateTime.Now;
			while (now - start < timeToWaitBeforeLoad)
			{
				now = DateTime.Now;
			}
			vertexes = new("", true);
			TickFeeder feeder = new();
		}
	}

	public class ProgressBar
	{
		int length;
		int progress;
		string backs;

		public ProgressBar(int length, string process = "")
		{
			this.length = length;
			Console.Write(process + ' ');
			backs = "\b\b";
			string output = "[";
			for (int i = 0; i < length; i++)
			{
				output += ' ';
				backs += '\b';
			}
			output += ']';
			Console.Write(output);
		}

		public void NextStep()
		{
			progress++;
			Console.Write(backs);
			string output = "[";
			int i;
			for (i = 0; i < progress; i++)
			{
				output += '#';
			}
			for (; i < length; i++)
			{
				output += ' ';
			}
			output += ']';
			Console.Write(output);
		}

		public void Close()
		{
			Console.Write(backs);
		}
	}

	public class ProgressCounter
	{
		int all;
		int progress;
		(int Left, int Top) beginPos;

		public ProgressCounter(int all)
		{
			this.all = all;
			progress = 0;
			beginPos = Console.GetCursorPosition();
			string outLine = progress.ToString() + " / " + all.ToString();
			Console.Write(outLine);
		}

		public void NextStep()
		{
			progress++;
			string outline = "";
			outline += progress.ToString() + " / " + all.ToString();
			var curPos = Console.GetCursorPosition();
			Console.SetCursorPosition(beginPos.Left, beginPos.Top);
			Console.Write(outline);
			Console.SetCursorPosition(curPos.Left, curPos.Top);
		}
	}
}
