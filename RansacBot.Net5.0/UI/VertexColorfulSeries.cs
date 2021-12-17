using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot.UI
{
	class VertexColorfulSeries
	{
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

		public VertexColorfulSeries()
		{
			throw new NotImplementedException();
		}
	}
}
