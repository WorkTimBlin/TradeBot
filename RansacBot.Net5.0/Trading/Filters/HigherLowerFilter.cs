using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading.Filters
{
	class HigherLowerFilter:IExtremumFilter
	{
		Tick high = new Tick(0, 0, double.MaxValue);
		Tick low = new Tick(0, 0, double.MinValue);
		public event ExtremumHandler NewExtremum;
		public void OnNewExtremum(Tick extremum, VertexType vertexType, Tick current)
		{
			bool permission = false;
			if (vertexType == VertexType.High)
			{
				permission = extremum.PRICE > high.PRICE;
				high = extremum;
			}
			if (vertexType == VertexType.Low)
			{
				permission = extremum.PRICE < low.PRICE;
				low = extremum;
			}
			if (permission) NewExtremum?.Invoke(extremum, vertexType, current);
		}
	}
}
