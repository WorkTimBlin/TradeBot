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
		LinearAxis axisX = new()
		{
			Position = AxisPosition.Bottom,
			Title = "Vertexes",
			LabelFormatter = (x) => x.ToString(),
			Key = "X"
		};
		LinearAxis axisY = new()
		{
			Position = AxisPosition.Left,
			Title = "Price",
			MajorGridlineStyle = LineStyle.Dot,
			LabelFormatter = (x) => x.ToString(),
			Key = "Y"
		};
		LineSeries vertexes = new()
		{
			Title = "vertexes",
		};

		int Level { get; }

		void InitialiseModel()
		{
			plotModel.Axes.Add(axisX);
			plotModel.Axes.Add(axisY);
		}

		void PrintNewRansac(Ransac ransac, int level)
		{
			if (level != this.Level) return;
			plotModel.Annotations.Add(new LineAnnotation()
			{
				Slope = ransac.Slope,
				Intercept = ransac.Intercept,
				MinimumX = ransac.firstTickIndex,
				MaximumX = ransac.EndTickIndex,
			});
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
