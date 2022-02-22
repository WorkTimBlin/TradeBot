using RansacBot.Trading;
using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.Assemblies
{
	class S2_ET_S2_TradeWithStopProvider : ITradeWithStopProvider, ITickProcessor
	{
		public event Action<TradeWithStop> NewTradeWithStop;

		public void OnNewTick(Tick tick)
		{
			throw new NotImplementedException();
		}

		RansacsSession session;
		RansacsCascade SCascade;
		RansacsCascade ETCascade;

		public S2_ET_S2_TradeWithStopProvider()
		{
			session = new(100);
			SCascade = new(session.vertexes, SigmaType.Sigma, 90);
			ETCascade = new(session.vertexes, SigmaType.Sigma, 90);
		}
	}
}
