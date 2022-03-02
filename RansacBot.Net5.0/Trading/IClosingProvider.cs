using System;

namespace RansacBot.Trading
{
	internal interface IClosingProvider
	{
		public event Action<double> ClosePercentOfLongs;
		public event Action<double> ClosePercentOfShorts;
	}
}