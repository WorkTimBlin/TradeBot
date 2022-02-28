using RansacBot.Trading;
using RansacBot.Trading.Hystory;
using System;

namespace RansacBot.Trading
{
	internal interface IHystoryOrderEvents
	{
		public event Action<HystoryOrder> OrderChanged;
	}
}