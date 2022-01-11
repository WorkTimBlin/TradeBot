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
			Title = "VertexIndexes",
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

		readonly RansacSeries ransacs = new RansacColorfulSeries();
		readonly LineSeries vertexes = new()
		{
			Title = "Vertexes",
			Color = OxyColors.Gray,
			MarkerFill = OxyColors.Blue,
			MarkerType = MarkerType.Circle,
			MarkerSize = 4,
			StrokeThickness = 1,
			Tag = "Vertexes",
			XAxisKey = "X",
			YAxisKey = "Y"
		};

		readonly int level;

		Ransac? currentRansac;
		public RansacsOxyPrinter(int level, RansacsCascade cascade)
		{
			this.level = level;
			cascade.NewRansac += OnNewRansac;
			cascade.RebuildRansac += OnRebuildRansac;
			cascade.StopRansac += OnStopRansac;
			cascade.NewVertex += OnNewVertex;
			plotModel.Axes.Add(axisX);
			plotModel.Axes.Add(axisY);
			plotModel.Series.Add(vertexes);
			ransacs.AddTo(plotModel);
		}

		public void OnNewRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			ransacs.BuildNewRansac(ransac);
			currentRansac = ransac;
			InvalidatePlot(true);
		}
		public void OnRebuildRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			ransacs.RebuildLastRansac(ransac);
			InvalidatePlot(true);
		}
		public void OnStopRansac(Ransac ransac, int level)
		{
			if (level != this.level) return;
			ransacs.StopLastRansac(ransac);
			currentRansac = null;
		}
		public void OnNewVertex(Tick tick)
		{
			vertexes.Points.Add(new DataPoint(tick.VERTEXINDEX, tick.PRICE));
			RedrawCurrentRansacToNewVertex();
			InvalidatePlot(true);
		}
		public void RedrawCurrentRansacToNewVertex()
        {
			if (currentRansac != null)
            {
				OnRebuildRansac(currentRansac, level);
            }
        }
		private void InvalidatePlot(bool updateData)
		{
			this.plotModel.InvalidatePlot(updateData);
		}
	}
}
