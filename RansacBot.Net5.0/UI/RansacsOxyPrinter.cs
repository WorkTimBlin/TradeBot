using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;
using RansacsRealTime;

namespace RansacBot.UI
{
	class RansacsOxyPrinter
	{
		public readonly PlotModel plotModel = new();
		readonly LinearAxis axisX = new()
		{
			Position = AxisPosition.Bottom,
			Title = "Vertexes",
			LabelFormatter = (x) => x.ToString(),
			Key = "X"
		};
		readonly LinearAxis axisY = new()
		{
			Position = AxisPosition.Left,
			Title = "Price",
			MajorGridlineStyle = LineStyle.Dot,
			LabelFormatter = (x) => x.ToString(),
			Key = "Y"
		};
		readonly LineSeries extremumVertexes = new()
		{
			Title = "Extremum-N",
			Color = OxyColors.Transparent,
			MarkerFill = OxyColors.Blue,
			MarkerType = MarkerType.Circle,
			MarkerSize = 4,
			Tag = "ExtremumsN",
			XAxisKey = "X",
			YAxisKey = "Y"
		};
		readonly LineSeries MonkeyVertexes = new()
		{
			Title = "Vertex-Monkey",
			Color = OxyColors.Transparent,
			MarkerFill = OxyColors.SkyBlue,
			MarkerType = MarkerType.Circle,
			MarkerSize = 4,
			Tag = "VertexesMonkey",
			XAxisKey = "X",
			YAxisKey = "Y"
		};
		readonly LineSeries VertexesLine = new()
		{
			Title = "Vertexes",
			Color = OxyColors.Gray,
			StrokeThickness = 1,
			Tag = "Vertexes",
			XAxisKey = "X",
			YAxisKey = "Y"
		};

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

		int Level { get; }

		void InitialiseModel()
		{
			plotModel.Axes.Add(axisX);
			plotModel.Axes.Add(axisY);
		}

		void PrintNewRansac(Ransac ransac, int level)
		{
			
		}

		LineSeries GetRansacLine(Ransac ransac)
		{
			LineSeries line = new()
			{
				Color = ransac.Slope > 0 ? OxyColors.Lime : OxyColors.Red,
				StrokeThickness = 3,
				XAxisKey = "X",
				YAxisKey = "Y",
				Tag = "ransac"
			};
			line.Points.Add(new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex)));
			line.Points.Add(new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex)));
			return line;
		}

		LineSeries GetSigmaLine(Ransac ransac)
		{
			LineSeries sigma = new()
			{
				Color = ransac.Slope > 0 ? OxyColors.Lime : OxyColors.Red,
				StrokeThickness = 2,
				LineStyle = LineStyle.Dot,
				XAxisKey = "X",
				YAxisKey = "Y",
				Tag = "sigma"
			};
			if (ransac.Slope > 0)
			{
				sigma.Points.Add(new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex) - ransac.Sigma));
				sigma.Points.Add(new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex) - ransac.Sigma));
			}
			else
			{
				sigma.Points.Add(new(ransac.firstTickIndex, ransac.GetValueAtPoint(ransac.firstTickIndex) + ransac.Sigma));
				sigma.Points.Add(new(ransac.LastTickIndex, ransac.GetValueAtPoint(ransac.LastTickIndex) + ransac.Sigma));
			}
			return sigma;
		}
	}
}
