using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacsRealTime
{
	public readonly struct RansacObservingParameters
	{
		public readonly SigmaType sigmaType;
		public readonly int level;

		public RansacObservingParameters(SigmaType sigmaType, int level)
		{
			this.sigmaType = sigmaType;
			this.level = level;
		}
	}
}
