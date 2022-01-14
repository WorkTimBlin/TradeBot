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
		public static readonly Quik quik;
		static QuikContainer()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			quik = new();
		}
	}
}
