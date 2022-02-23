using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class MaximinStopPlacer:IStopPlacer
	{
		double min = 0;
		double max = 0;
		readonly int level;
		bool wasCurrentRansacRaising = false;
		Ransac? currentRansac;
		Ransac? previousRansac;
		readonly Vertexes vertexes;
		
		public event Action<TradeWithStop> NewTradeWithStop;

		public MaximinStopPlacer(RansacsCascade cascade, int level)
		{
			this.level = level;
			this.vertexes = cascade.GetVertexes();
			vertexes.NewVertex += OnNewVertex;
			cascade.NewRansac += OnNewRansac;
			cascade.RebuildRansac += OnRebuildRansac;
			cascade.StopRansac += OnStopRansac;
		}

		public void OnNewTrade(Trade trade)
		{
			double stopPrice = 0;
			if(trade.direction == TradeDirection.buy && min < trade.price)
			{
				stopPrice = min;
			}
			else if(trade.direction == TradeDirection.sell && max > trade.price)
			{
				stopPrice = max;
			}
			if(stopPrice != 0)
			{
				NewTradeWithStop?.Invoke(new TradeWithStop(trade, stopPrice));
			}
		}

		void OnNewVertex(Tick tick, VertexType type)
		{
			if(currentRansac != null)
			{
				if (currentRansac.Slope > 0)
				{
					if (tick.PRICE < min) min = tick.PRICE;
				}
				else
				{
					if (tick.PRICE > max) max = tick.PRICE;
				}
			}
		}
		void OnNewRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			currentRansac = ransac;
			if (currentRansac.Slope > 0)
			{
				min = vertexes.vertexList[vertexes.GetIndexOfMinTickInRansac(currentRansac)].PRICE;
				wasCurrentRansacRaising = true;
			}
			else
			{
				max = vertexes.vertexList[vertexes.GetIndexOfMaxTickInRansac(currentRansac)].PRICE;
				wasCurrentRansacRaising = false;
			}
		}
		void OnRebuildRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			if (wasCurrentRansacRaising)
			{
				if (ransac.Slope > 0) return;
				max = vertexes.vertexList[vertexes.GetIndexOfMaxTickInRansac(ransac)].PRICE;
				min = previousRansac != null ? vertexes.vertexList[vertexes.GetIndexOfMinTickInRansac(previousRansac)].PRICE : 0;
				wasCurrentRansacRaising = false;
			}
			else
			{
				if (ransac.Slope <= 0) return;
				min = vertexes.vertexList[vertexes.GetIndexOfMinTickInRansac(ransac)].PRICE;
				max = previousRansac != null ? vertexes.vertexList[vertexes.GetIndexOfMaxTickInRansac(previousRansac)].PRICE : 0;
				wasCurrentRansacRaising = true;
			}
		}
		void OnStopRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			previousRansac = ransac;
			currentRansac = null;
			if(ransac.Slope > 0) 
				max = vertexes.vertexList[vertexes.GetIndexOfMaxTickInRansac(ransac)].PRICE;
			else 
				min = vertexes.vertexList[vertexes.GetIndexOfMinTickInRansac(ransac)].PRICE;
		}
	}
}
