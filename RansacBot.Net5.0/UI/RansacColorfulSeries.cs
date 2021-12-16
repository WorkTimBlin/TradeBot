using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.Series;
using OxyPlot;
using RansacsRealTime;
using System.Collections;

namespace RansacBot.UI
{
	class RansacColorfulSeries : RansacSeries
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

		public override int Count => (ransacsFalling.Points.Count + ransacsRaising.Points.Count) / 2;
		public override void Add(Ransac ransac)
		{
			if(IsRaising(ransac))
			{
				AddRansacRaising(ransac);
				AddSigmaRaising(ransac);
			}
			else
			{
				AddRansacFalling(ransac);
				AddSigmaFalling(ransac);
			}
		}
		public override void Clear()
		{
			ransacsFalling.Points.Clear();
			sigmasFalling.Points.Clear();
			ransacsRaising.Points.Clear();
			sigmasRising.Points.Clear();
		}

		public override void Render(IRenderContext rc)
		{
			ransacsFalling.Render(rc);
			ransacsRaising.Render(rc);
			sigmasFalling.Render(rc);
			sigmasRising.Render(rc);
		}

		private void AddRansacRaising(Ransac ransac)
		{
			var startEndPoints = GetStartEndPoints(ransac);
			ransacsRaising.Points.Add(startEndPoints.start);
			ransacsRaising.Points.Add(startEndPoints.end);
		}
		private void AddRansacFalling(Ransac ransac)
		{
			var startEndPoints = GetStartEndPoints(ransac);
			ransacsFalling.Points.Add(startEndPoints.start);
			ransacsFalling.Points.Add(startEndPoints.end);
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

	public abstract class RansacSeries : XYAxisSeries
	{
		public abstract int Count { get; }
		public abstract void Add(Ransac ransac);
		public abstract void Clear();
		protected static bool IsRaising(Ransac ransac)
		{
			return ransac.Slope > 0;
		}
		protected (DataPoint start, DataPoint end) GetStartEndPoints(Ransac ransac)
		{
			return (
				new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex)),
				new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex)));
		}
	}
}
