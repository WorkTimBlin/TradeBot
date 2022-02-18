using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;

namespace RansacBot.Trading
{
	class RansacDirectionTradeFilter:ITradeFilter
	{
		readonly int ransacLevel;
		RansacType currentRansac = RansacType.None;
		public event TradeHandler NewTrade;

		public RansacDirectionTradeFilter(RansacsCascade cascade, int ransacLevel)
		{
			this.ransacLevel = ransacLevel;
			cascade.NewRansac += OnNewRansac;
			cascade.RebuildRansac += OnRebuildRansac;
			cascade.StopRansac += OnStopRansac;
		}
		public void OnNewTrade(Trade trade)
		{
			if (
				currentRansac == RansacType.Falling && trade.direction == TradeDirection.sell ||
				currentRansac == RansacType.Raising && trade.direction == TradeDirection.buy)
				NewTrade?.Invoke(trade);
		}
		public void OnNewRansac(Ransac ransac, int level)
		{
			if (level != ransacLevel) return;
			ChangeRansac(ransac);
		}
		public void OnRebuildRansac(Ransac ransac, int level)
		{
			OnNewRansac(ransac, level);
		}
		public void OnStopRansac(Ransac ransac, int level)
		{
			if (level != ransacLevel) return;
			currentRansac = RansacType.None;
		}
		private void ChangeRansac(Ransac ransac)
		{
			if (ransac.Slope > 0)
			{
				currentRansac = RansacType.Raising;
			}
			else
			{
				currentRansac = RansacType.Falling;
			}
		}
		private enum RansacType
		{
			None,
			Raising, 
			Falling
		}
	}

	class RansacDirectionExtremumFilter:IExtremumFilter
	{
		public event ExtremumHandler NewExtremum;

		readonly int ransacLevel;
		RansacType currentRansac = RansacType.None;

		public RansacDirectionExtremumFilter(RansacsCascade cascade, int ransacLevel)
		{
			this.ransacLevel = ransacLevel;
			cascade.NewRansac += OnNewRansac;
			cascade.RebuildRansac += OnRebuildRansac;
			cascade.StopRansac += OnStopRansac;
		}
		public void OnNewExtremum(Tick tick, VertexType vertexType, Tick current)
		{
			if (!
				(currentRansac == RansacType.Raising && vertexType == VertexType.High
				||
				currentRansac == RansacType.Falling && vertexType == VertexType.Low)
				)
				NewExtremum?.Invoke(tick, vertexType, current);
		}
		public void OnNewRansac(Ransac ransac, int level)
		{
			if (level != ransacLevel) return;
			ChangeRansac(ransac);
		}
		public void OnRebuildRansac(Ransac ransac, int level)
		{
			OnNewRansac(ransac, level);
		}
		public void OnStopRansac(Ransac ransac, int level)
		{
			if (level != ransacLevel) return;
			currentRansac = RansacType.None;
		}
		private void ChangeRansac(Ransac ransac)
		{
			if (ransac.Slope > 0)
			{
				currentRansac = RansacType.Raising;
			}
			else
			{
				currentRansac = RansacType.Falling;
			}
		}
		private enum RansacType
		{
			None,
			Raising,
			Falling
		}
	}
}
