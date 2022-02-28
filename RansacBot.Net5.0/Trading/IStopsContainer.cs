using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	interface IStopsContainer
	{
		public IEnumerable<string> GetLongs();
		public IEnumerable<string> GetShorts();
		public IEnumerable<string> GetExecuted();
	}
}
