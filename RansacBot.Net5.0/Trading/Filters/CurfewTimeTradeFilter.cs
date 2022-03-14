using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading.Filters
{
	public class CurfewTimeTradeFilter : ITradeFilter
	{
		public event Action<Trade> NewTrade;
		TimeSpan closingTime;
		TimeSpan openingTime;
		Func<DateTime> GetDateTime;

		public CurfewTimeTradeFilter(TimeSpan closingTime, TimeSpan openingTime) : 
			this(closingTime, openingTime, () => DateTime.Now) { }

		public CurfewTimeTradeFilter(TimeSpan closingTime, TimeSpan openingTime, Func<DateTime> dateTimeGetter)
		{
			if ((int)closingTime.TotalDays > 0 || (int)openingTime.TotalDays > 0) 
				throw new ArgumentException("closing time and opening time must be times of day");
			this.closingTime = closingTime;
			this.openingTime = openingTime;
			GetDateTime = dateTimeGetter;
		}

		public void OnNewTrade(Trade trade)
		{
			DateTime now = GetDateTime();
			if (now.TimeOfDay < closingTime || now.TimeOfDay > openingTime) NewTrade?.Invoke(trade);
		}
	}
}
