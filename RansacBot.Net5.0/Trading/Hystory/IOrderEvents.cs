using RansacBot.Trading;
using System;

namespace RansacBot.Trading
{
	internal interface IOrderEvents
	{
		public event Action<HystoryOrder> OrderChanged;
	}
}