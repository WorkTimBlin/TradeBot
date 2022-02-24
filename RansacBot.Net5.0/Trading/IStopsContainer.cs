using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Trading
{
	interface IStopsContainer
	{
		public List<string> GetLongs();
		public List<string> GetShorts();
	}
}
