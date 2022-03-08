using RansacBot.Trading;
using RansacsRealTime;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RansacBot
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//UI.FormChoice choiceForm = new();
			Application.Run(new UI.FlexibleHystoryTestForm());
		}
	}
}
