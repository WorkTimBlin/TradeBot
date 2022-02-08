using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RansacsRealTime;
using RansacBot.Trading;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
namespace RansacBot.UI
{
	class RansacsOxyPrinterWithTrades : RansacsOxyPrinter
	{
		public readonly ScatterSeries longs = new()
		{
			Title = "Longs",
			MarkerFill = OxyColors.Lime,
			MarkerType = MarkerType.Custom, 
			MarkerOutline = new ScreenPoint[] { new(0, 0), new(0, -3), new(-1, -3), new(1, -4), new(3, -3), new(2, -3), new(2,0)},
			MarkerSize = 4,
			MarkerStrokeThickness = 1,
			Tag = "Longs",
			XAxisKey = "X",
			YAxisKey = "Y"
		};

		public readonly ScatterSeries shorts = new()
		{
			Title = "Shorts",
			MarkerFill = OxyColors.Red,
			MarkerType = MarkerType.Custom,
			MarkerOutline = new ScreenPoint[] { new(0, 0), new(0, 3), new(-1, 3), new(1, 4), new(3, 3), new(2, 3), new(2, 0) },
			MarkerSize = 4,
			MarkerStrokeThickness = 1,
			Tag = "Shorts",
			XAxisKey = "X",
			YAxisKey = "Y"
		};

		public readonly ScatterSeries stops = new()
		{
			Title = "Stops",
			MarkerFill = OxyColors.Yellow,
			MarkerType = MarkerType.Square,
			MarkerSize = 4,
			MarkerStrokeThickness = 1,
			Tag = "Stops",
			XAxisKey = "X",
			YAxisKey = "Y"

		};
		
		public RansacsOxyPrinterWithTrades(int level, RansacsCascade cascade) : base(level, cascade)
		{
			plotModel.Series.Add(longs);
			plotModel.Series.Add(shorts);
			plotModel.Series.Add(stops);
		}

		TradeWithStop? lastTradeWithStop = null;
		int lastExtremumVertexIndex;
		bool lastExtremumFound = false;
		double priceWhereLastExtremumFound;

		public void OnNewTradeWithStop(TradeWithStop tradeWithStop)
		{
			lastTradeWithStop = tradeWithStop;
			Console.WriteLine("trade with stop " + tradeWithStop.price.ToString());
			CheckIfTradeWithStopHappenedThenAddToPlot();
		}

		public void OnNewExtremum(Tick extremum, VertexType vertexType, Tick current)
		{
			lastExtremumVertexIndex = extremum.VERTEXINDEX;
			lastExtremumFound = true;
			priceWhereLastExtremumFound = current.PRICE;
			CheckIfTradeWithStopHappenedThenAddToPlot();
		}

		private void CheckIfTradeWithStopHappenedThenAddToPlot()
		{
			if (lastExtremumFound && lastTradeWithStop != null)
			{
				if (lastTradeWithStop.direction == TradeDirection.buy)
				{
					longs.Points.Add(new ScatterPoint(lastExtremumVertexIndex + 0.5, lastTradeWithStop.price));
					stops.Points.Add(new ScatterPoint(lastExtremumVertexIndex + 0.5, lastTradeWithStop.stop.price));
				}
				else
				{
					shorts.Points.Add(new ScatterPoint(lastExtremumVertexIndex + 0.5, lastTradeWithStop.price));
					stops.Points.Add(new ScatterPoint(lastExtremumVertexIndex + 0.5, lastTradeWithStop.stop.price));
				}

				lastTradeWithStop = null;
				lastExtremumFound = false;
				lastExtremumVertexIndex = 0;
				priceWhereLastExtremumFound = 0.0;
			}
		}

		public void OnClosePos(decimal stopPrice)
		{
			for(int i = 0; i < stops.Points.Count; i++)
			{
				if(stops.Points[i].Y == (double)stopPrice)
				{
					stops.Points.RemoveAt(i);
					return;
				}
			}
			throw new Exception("couldn't find corresponding stop");
		}

	}
}
