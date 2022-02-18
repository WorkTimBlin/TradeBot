using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp;
using RansacsRealTime;

namespace RansacBot
{
	interface ITickByParamProvider : IProviderByParam<Tick>
	{
		//void Subscribe(Param param, TickHandler handler);
		//void Unsubscribe(Param param, TickHandler handler);
	}

	interface IProviderByParam<T>
	{
		void Subscribe(Param param, Action<T> handler);
		void Unsubscribe(Param param, Action<T> handler);
	}
}
