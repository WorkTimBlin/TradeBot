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
		void Subscribe(Instrument instrument, TickHandler handler);
		void Unsubscribe(Instrument instrument, TickHandler handler);
	}
}
