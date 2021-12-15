using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.Series;
using OxyPlot;
using RansacsRealTime;

namespace RansacBot.UI
{
	class RansacSeries1:Series
	{
		readonly LineSegmentSeries ransacsUp = new()
		{
			Color = OxyColors.Lime,
			StrokeThickness = 3,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "ransac"
		};
		readonly LineSegmentSeries ransacsDown = new()
		{
			Color = OxyColors.Red,
			StrokeThickness = 3,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "ransac"
		};
		readonly LineSegmentSeries sigmasUp = new()
		{
			Color = OxyColors.Lime,
			StrokeThickness = 2,
			LineStyle = LineStyle.Dot,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "sigma"
		};
		readonly LineSegmentSeries sigmasDown = new()
		{
			Color = OxyColors.Red,
			StrokeThickness = 2,
			LineStyle = LineStyle.Dot,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "sigma"
		};

		protected override bool AreAxesRequired()
		{
			return true;
		}
		protected override void EnsureAxes()
		{
			if (ransacsUp.XAxis == null || ransacsUp.YAxis == null) throw new Exception("Axes are not defined");
		}
		protected override bool IsUsing(Axis axis)
		{
			foreach(thisAxis in this.Axes)
		}
		protected override void SetDefaultValues();
		protected override void UpdateAxisMaxMin();
		protected override void UpdateData();
		//protected override void UpdateMaxMin();
	}
	class RansacsColourfulContainer:IList<Ransac>
	{
		readonly LineSegmentSeries ransacsRaising = new()
		{
			Color = OxyColors.Lime,
			StrokeThickness = 3,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "ransac"
		};
		readonly LineSegmentSeries ransacsFalling = new()
		{
			Color = OxyColors.Red,
			StrokeThickness = 3,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "ransac"
		};
		readonly LineSegmentSeries sigmasRising = new()
		{
			Color = OxyColors.Lime,
			StrokeThickness = 2,
			LineStyle = LineStyle.Dot,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "sigma"
		};
		readonly LineSegmentSeries sigmasFalling = new()
		{
			Color = OxyColors.Red,
			StrokeThickness = 2,
			LineStyle = LineStyle.Dot,
			XAxisKey = "X",
			YAxisKey = "Y",
			Tag = "sigma"
		};
		public void Add(Ransac ransac)
		{
			if(ransac.Slope > 0)
			{
				AddRansacRaising(ransac);
				AddRansacFalling(ransac);
			}
		}
		private void AddRansacRaising(Ransac ransac)
		{
			ransacsRaising.Points.Add(new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex)));
			ransacsRaising.Points.Add(new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex)));
		}
		private void AddRansacFalling(Ransac ransac)
		{
			ransacsFalling.Points.Add(new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex)));
			ransacsFalling.Points.Add(new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex)));
		}
		private void AddSigmaRaising(Ransac ransac)
		{
			sigmasRising.Points.Add(new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex) - ransac.Sigma));
			sigmasRising.Points.Add(new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex) - ransac.Sigma));
		}
		private void AddSigmaFalling(Ransac ransac)
		{
			sigmasFalling.Points.Add(new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex) + ransac.Sigma));
			sigmasFalling.Points.Add(new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex) + ransac.Sigma));
		}
	}
	abstract class RansacSeries : Series
	{
		public abstract void AddRansac(Ransac ransac);
		public abstract void RemoveRansac()
	}
}
