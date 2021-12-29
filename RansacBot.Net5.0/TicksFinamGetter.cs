using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	interface ITicksHystoryGetter
	{
		IList<Tick> GetTicks(DateTime fromDate, DateTime toDate);
	}
	class TicksFinamGetter:ITicksHystoryGetter
	{
		public IList<Tick> GetTicks(DateTime fromDate, DateTime toDate)
		{
			return new TicksLazyParser(FinamDataLoader.RawFinamHystory.GetTickLines(fromDate, toDate));
		}
	}
}
