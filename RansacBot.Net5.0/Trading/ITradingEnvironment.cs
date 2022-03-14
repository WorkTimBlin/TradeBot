using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	interface IDateTimeProvider
	{
		public DateTime DateTime { get; }
	}
	interface ITradingEnvironment : IDateTimeProvider
	{

	}
}
