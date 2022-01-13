using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot
{
	interface ITickByInstrumentProvider
	{
		void Subscribe(Param instrument, TickHandler handler);
		void Unsubscribe(Param instrument, TickHandler handler);
	}
}
