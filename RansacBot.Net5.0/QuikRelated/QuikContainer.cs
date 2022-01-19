using QuikSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	public static class QuikContainer
	{
		private static Quik _quik;
		public static Quik Quik { get => _quik; }
		static QuikContainer()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			_quik = new();
		}

		public static void ReloadQuik()
		{
			_quik.Service.QuikService.Stop();
			_quik.Service.QuikService.Start();
		}
	}
}
