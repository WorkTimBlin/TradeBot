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
		readonly LineSegmentSeries ransacsRising = new()
		{
			Color = OxyColors.Lime,
			StrokeThickness = 3,
			Tag = "ransac"
		};
		readonly LineSegmentSeries ransacsFalling = new()
		{
			Color = OxyColors.Red,
			StrokeThickness = 3,
			Tag = "ransac"
		};
		readonly LineSegmentSeries sigmasRising = new()
		{
			Color = OxyColors.Lime,
			StrokeThickness = 2,
			LineStyle = LineStyle.Dot,
			Tag = "sigma"
		};
		readonly LineSegmentSeries sigmasFalling = new()
		{
			Color = OxyColors.Red,
			StrokeThickness = 2,
			LineStyle = LineStyle.Dot,
			Tag = "sigma"
		};
		private bool wasLastRaising;

		public override int Count => (ransacsFalling.Points.Count + ransacsRising.Points.Count) / 2;
		public override void BuildNewRansac(Ransac ransac)
		{
			if(IsRaising(ransac))
			{
				AddRansacRaising(ransac);
				AddSigmaRaising(ransac);
				wasLastRaising = true;
			}
			else
			{
				AddRansacFalling(ransac);
				AddSigmaFalling(ransac);
				wasLastRaising = false;
			}
		}
		public override void RebuildLastRansac(Ransac ransac)
		{
			RemoveLast();
			BuildNewRansac(ransac);
		}
		public override void StopLastRansac(Ransac ransac)
		{
			RebuildLastRansac(ransac);
		}
		public override void Clear()
		{
			ransacsFalling.Points.Clear();
			sigmasFalling.Points.Clear();
			ransacsRising.Points.Clear();
			sigmasRising.Points.Clear();
		}

		public override void AddTo(PlotModel model)
		{
			model.Series.Add(ransacsFalling);
			model.Series.Add(ransacsRising);
			model.Series.Add(sigmasFalling);
			model.Series.Add(sigmasRising);
		}
		public override void RemoveFrom(PlotModel model)
		{
			model.Series.Remove(ransacsFalling);
			model.Series.Remove(ransacsRising);
			model.Series.Remove(sigmasFalling);
			model.Series.Remove(sigmasRising);
		}

		private void RemoveLast()
		{
			if (wasLastRaising)
			{
				RemoveLastAt(ransacsRising);
				RemoveLastAt(sigmasRising);
			}
			else
			{
				RemoveLastAt(ransacsFalling);
				RemoveLastAt(sigmasFalling);
			}
		}

		private void RemoveLastAt(LineSegmentSeries series)
		{
			series.Points.RemoveAt(series.Points.Count - 1);
			series.Points.RemoveAt(series.Points.Count - 1);
		}
		private void AddRansacRaising(Ransac ransac)
		{
			var startEndPoints = GetStartEndPoints(ransac);
			ransacsRising.Points.Add(startEndPoints.start);
			ransacsRising.Points.Add(startEndPoints.end);
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

	public abstract class RansacSeries 
	{
		public abstract int Count { get; }
		public abstract void BuildNewRansac(Ransac ransac);
		public abstract void RebuildLastRansac(Ransac ransac);
		public abstract void StopLastRansac(Ransac ransac);
		public abstract void Clear();
		public abstract void AddTo(PlotModel model);
		public abstract void RemoveFrom(PlotModel model);
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
