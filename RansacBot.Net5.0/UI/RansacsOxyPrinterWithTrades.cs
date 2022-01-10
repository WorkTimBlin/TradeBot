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
            MarkerType = MarkerType.Triangle,
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
            MarkerType = MarkerType.Triangle,
            MarkerSize = 4,
            MarkerStrokeThickness = 1,
            Tag = "Shorts",
            XAxisKey = "X",
            YAxisKey = "Y"
        };
        
        public RansacsOxyPrinterWithTrades(int level, RansacsCascade cascade) : base(level, cascade)
        {
            plotModel.Series.Add(longs);
            plotModel.Series.Add(shorts);
        }

        Trade? lastTrade = null;
        int lastExtremumVertexIndex;
        bool lastExtremumFound = false;
        double priceWhereLastExtremumFound;

        public void OnNewTrade(Trade trade)
        {
            lastTrade = trade;
            CheckIfTradeHappenedThenAddToPlot();
        }

        public void OnNewExtremum(Tick extremum, VertexType vertexType, Tick current)
        {
            lastExtremumVertexIndex = extremum.VERTEXINDEX;
            lastExtremumFound = true;
            priceWhereLastExtremumFound = current.PRICE;
            CheckIfTradeHappenedThenAddToPlot();
        }

        private void CheckIfTradeHappenedThenAddToPlot()
        {
            if (lastExtremumFound && lastTrade != null && lastTrade.price == priceWhereLastExtremumFound)
            {
                if (lastTrade.direction == TradeDirection.buy) longs.Points.Add(new ScatterPoint(lastExtremumVertexIndex + 0.5, lastTrade.price));
                else shorts.Points.Add(new ScatterPoint(lastExtremumVertexIndex + 0.5, lastTrade.price));
                
                lastTrade = null;
                lastExtremumFound = false;
                lastExtremumVertexIndex = 0;
                priceWhereLastExtremumFound = 0.0;
            }
        }


    }
}
