using RansacRealTime;
using System;
using System.Windows.Forms;

namespace RansacBot
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		/*
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMain());
		}
		*/

		static void Main()
		{
			ObservingSession session = new(new Instrument() { classCode = "SPBFUT", securityCode = "RIZ1"}, 100);
			session.AddNewRansacsCascade(TypeSigma.Sigma);
			session.ransacs.vertexes.cascades[0].NewRansac += (Ransac ransac, int level) => { Console.WriteLine("new ransac builded on level " + (level+2).ToString()); };
			session.ransacs.vertexes.cascades[0].StopRansac += (Ransac ransac, int level) => { Console.WriteLine("ransac stopped on level " + (level + 2).ToString()); };
			session.ransacs.vertexes.cascades[0].RebuildRansac += (Ransac ransac, int level) => { Console.WriteLine("ransac rebuilded on level " + (level + 2).ToString()); };
			while (true) { }
		}
	}
}
