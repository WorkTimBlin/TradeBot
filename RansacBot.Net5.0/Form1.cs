using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using QuikSharp.DataStructures;
using RansacRealTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace RansacBot.Net5._0
{
    public partial class FormMain : Form
    {
        delegate void TextBoxTextDelegate(TextBox tb, string text);
        private int currentLevel;
        private int currentHystory;

        public FormMain()
        {
            InitializeComponent();
        }

        #region Обработчики элементов управления

        private void FormMain_Load(object sender, EventArgs e)
        {
            LOGGER.NewMessage += Logger_NewMessage;
            timer.Stop();
        }
        private void LoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormLogin formLogin = new();
                formLogin.ShowDialog();

                if (formLogin.DialogResult == DialogResult.OK)
                {
                    UpdateStaticParams();
                    UpdateCurrentParams();
                    LoginToolStripMenuItem.Enabled = false;
                    OnToolStripMenuItem.Enabled = true;

                    LOGGER.Trace("Подключение к Quik выполнено успешно!");
                }
                else
                {
                    LOGGER.Trace("Подключение отменено!");
                }
            }
            catch (Exception ex)
            {
                ClearInterface();
                LoginToolStripMenuItem.Enabled = true;
                OnToolStripMenuItem.Enabled = false;
                LOGGER.Trace("Login_Click(): Exception - " + ex.Message);
            }
        }
        private void OnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializationModel();

            Connector.Subscribe(ToolObserver.CurrentTool.ClassCode, ToolObserver.CurrentTool.SecurityCode, ToolObserver.Data.MonkeyNFilter.OnNewTick);

            Connector.NewPrice = Connector_NewPrice;
            ToolObserver.Data.MonkeyNFilter.NewVertex += ToolObserver.Data.Vertexes.OnNewVertex;
            ToolObserver.Data.MonkeyNFilter.NewVertex += MonkeyNFilter_NewVertex;

            cbRansac.Items.AddRange(ToolObserver.Data.Vertexes.Hystories.Select(x => x.Type.ToString()).ToArray());
            cbRansac.SelectedIndex = 0;
            nmcLevelRansac.Value = ToolObserver.Data.Vertexes.Hystories[cbRansac.SelectedIndex].MaxLevel + 2;

            cbRansac.Visible = true;
            nmcLevelRansac.Visible = true;
            OnToolStripMenuItem.Enabled = false;
            LOGGER.Trace("Восстание машин активировано!");
            timer.Start();
        }
        private void CbRansac_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentHystory = cbRansac.SelectedIndex;
            var level = ToolObserver.Data.Vertexes.Hystories[currentHystory].Levels.Count == 0 ? null : ToolObserver.Data.Vertexes.Hystories[currentHystory].Levels[^1];
           
            for (int i = 0; i < ToolObserver.Data.Vertexes.Hystories.Count; i++)
            {
                ToolObserver.Data.Vertexes.Hystories[i].RebuildRansac -= RansacHystory_RebuildRansac;
                ToolObserver.Data.Vertexes.Hystories[i].NewRansac -= RansacHystory_NewRansac;
            }

            ToolObserver.Data.Vertexes.Hystories[currentHystory].RebuildRansac += RansacHystory_RebuildRansac;
            ToolObserver.Data.Vertexes.Hystories[currentHystory].NewRansac += RansacHystory_NewRansac;

            NmcLevelRansac_ValueChanged(sender, e);
            nmcLevelRansac.Maximum = ToolObserver.Data.Vertexes.Hystories[currentHystory].MaxLevel + 2;
        }
        private void NmcLevelRansac_ValueChanged(object sender, EventArgs e)
        {
            currentLevel = (int)nmcLevelRansac.Value - 2;
            ClearRansacs();

            if (currentLevel < ToolObserver.Data.Vertexes.Hystories[currentHystory].Levels.Count)
            {
                PrintRansacs(ToolObserver.Data.Vertexes.Hystories[currentHystory].Levels[currentLevel].GetRansacs());
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (OxyChart.InvokeRequired)
            {
                OxyChart.Invoke(new System.Action(() => { OxyChart.InvalidatePlot(true); }));
                LOGGER.Trace("Необычная отрисовка");
            }
            else
                OxyChart.InvalidatePlot(true);
        }

        #endregion

        #region События

        private void MonkeyNFilter_NewVertex(Tick tick, VertexType vertexType)
        {
            ((LineSeries)OxyChart.Model.Series[0]).Points.Add(new DataPoint(tick.VERTEXINDEX, tick.PRICE));

            if (vertexType == VertexType.Monkey)
                ((LineSeries)OxyChart.Model.Series[2]).Points.Add(new DataPoint(tick.VERTEXINDEX, tick.PRICE));
            else
                ((LineSeries)OxyChart.Model.Series[1]).Points.Add(new DataPoint(tick.VERTEXINDEX, tick.PRICE));

            if (ToolObserver.Data.Vertexes.Hystories[currentHystory].Levels[currentLevel].IsBuilding)
            {
                RansacHystory_RebuildRansac(ToolObserver.Data.Vertexes.Hystories[currentHystory].Levels[currentLevel].GetRansacs().Last(), currentLevel);
            }

            LOGGER.Trace("Найдена новая вершина: " + tick.PRICE + " | Индекс: " + tick.VERTEXINDEX);
        }
        private void RansacHystory_NewRansac(Ransac ransac, int level)
        {
            if (level != currentLevel)
                return;

            while ((OxyChart.Model.Series.Count - 4) > 20)
            {
                OxyChart.Model.Series.RemoveAt(4);
                OxyChart.Model.Series.RemoveAt(4);
            }

            PrintRansac(ransac);
            LOGGER.Trace("Найден новый ранзак: " + (ransac.EndIndexTick - 1));
        }
        private void RansacHystory_RebuildRansac(Ransac ransac, int level)
        {
            if (level != currentLevel)
                return;

            if (OxyChart.Model.Series.Count > 4)
            {
                OxyChart.Model.Series.RemoveAt(OxyChart.Model.Series.Count - 1);
                OxyChart.Model.Series.RemoveAt(OxyChart.Model.Series.Count - 1);
            }

            PrintRansac(ransac);
            LOGGER.Trace("Текущий ранзак перестроен: " + (ransac.EndIndexTick - 1));
        }
        private void Connector_NewPrice(double price)
        {
            ((LineSeries)OxyChart.Model.Series[3]).Points[0] = new DataPoint(((LineSeries)OxyChart.Model.Series[0]).Points.Count, price);
        }
        private void Logger_NewMessage(string message)
        {
            statusStrip.Items["toolStripStatus"].Text = message;
        }

        #endregion

        #region Вспомогательные функции

        private void UpdateStaticParams()
        {
            if (ToolObserver.CurrentTool != null)
            {
                TextToTextBox(tbClientCode, ToolObserver.CurrentTool.ClientCode.ToString());
                TextToTextBox(tbAccountID, ToolObserver.CurrentTool.AccountID.ToString());
                TextToTextBox(tbClassCode, ToolObserver.CurrentTool.ClassCode.ToString());
                TextToTextBox(tbFirmID, ToolObserver.CurrentTool.FirmID.ToString());
                TextToTextBox(tbSecCode, ToolObserver.CurrentTool.SecurityCode.ToString());
                TextToTextBox(tbShortName, ToolObserver.CurrentTool.Name.ToString());
                TextToTextBox(tbStep, ToolObserver.CurrentTool.Step.ToString());
                TextToTextBox(tbStepPrice, ToolObserver.CurrentTool.PriceStep.ToString());
                TextToTextBox(tbGoBuy, ToolObserver.CurrentTool.GOBuy.ToString());
                TextToTextBox(tbGoSell, ToolObserver.CurrentTool.GOSell.ToString());
            }
            else ClearInterface();
        }
        private void UpdateCurrentParams()
        {
            if (ToolObserver.CurrentTool != null)
            {
                TextToTextBox(tbAvailableMax, Connector.quik.Trading.CalcBuySell(ToolObserver.CurrentTool.ClassCode, ToolObserver.CurrentTool.SecurityCode, ToolObserver.CurrentTool.ClientCode, ToolObserver.CurrentTool.AccountID, 0, true, true).Result.Qty + " | " +
                    Connector.quik.Trading.CalcBuySell(ToolObserver.CurrentTool.ClassCode, ToolObserver.CurrentTool.SecurityCode, ToolObserver.CurrentTool.ClientCode, ToolObserver.CurrentTool.AccountID, 0, false, true).Result.Qty);
                TextToTextBox(tbCurrentPos, "0 | 0");
                
                FuturesClientHolding futuresClient = Connector.quik.Trading.GetFuturesHolding(ToolObserver.CurrentTool.FirmID, ToolObserver.CurrentTool.AccountID, ToolObserver.CurrentTool.SecurityCode, 1).Result;
                //FuturesLimits futuresLimits = Connector.quik.Trading.GetFuturesLimit(Trader.Tool.FirmID, Trader.Tool.AccountID, 0, "").Result;


                if (futuresClient != null)
                {
                    TextToTextBox(tbVarMargin, futuresClient.varMargin.ToString());
                    TextToTextBox(tbBalanceNoMargin, "-");
                    TextToTextBox(tbBalance, "-");
                    TextToTextBox(tbBlock, "-");
                    TextToTextBox(tbAvailableFunds, "-");
                }
                else
                {
                    TextToTextBox(tbVarMargin, "-");
                    TextToTextBox(tbBalanceNoMargin, "-");
                    TextToTextBox(tbBalance, "-");
                    TextToTextBox(tbBlock, "-");
                    TextToTextBox(tbAvailableFunds, "-");
                }
            }
            else ClearInterface();
        }
        private void ClearInterface()
        {
            TextToTextBox(tbClientCode, "");
            TextToTextBox(tbAccountID, "");
            TextToTextBox(tbClassCode, "");
            TextToTextBox(tbFirmID, "");
            TextToTextBox(tbSecCode, "");
            TextToTextBox(tbShortName, "");
            TextToTextBox(tbStep, "");
            TextToTextBox(tbStepPrice, "");
            TextToTextBox(tbGoBuy, "");
            TextToTextBox(tbGoSell, "");

            TextToTextBox(tbAvailableMax, "");
            TextToTextBox(tbCurrentPos, "");
            TextToTextBox(tbVarMargin, "");
            TextToTextBox(tbBalanceNoMargin, "");
            TextToTextBox(tbBalance, "");
            TextToTextBox(tbBlock, "");
            TextToTextBox(tbAvailableFunds, "");
        }
        private void InitializationModel()
        {
            PlotModel plot = new()
            {
                Title = "MonkeyN - " + ToolObserver.N,
                TitleColor = OxyColors.Black
            };

            LinearAxis axisX = new()
            {
                Position = AxisPosition.Bottom,
                Title = "Ticks",
                //TextColor = OxyColors.White,
                //TitleColor = OxyColors.White,
                //AxislineColor = OxyColors.White,
                //TicklineColor = OxyColors.White,
                //MinorTicklineColor = OxyColors.White,
                LabelFormatter = (x) => x.ToString(),
                Key = "X"
            };
            LinearAxis axisY = new()
            {
                Position = AxisPosition.Left,
                Title = "Price",
                MajorGridlineStyle = LineStyle.Dot,
                //MinorTickSize = 0,
                //TextColor = OxyColors.White,
                //TitleColor = OxyColors.White,
                //AxislineColor = OxyColors.White,
                //TicklineColor = OxyColors.White,
                //MajorGridlineColor = OxyColors.White,
                LabelFormatter = (x) => x.ToString(),
                Key = "Y"
            };
            LineSeries extremumsN = new()
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

            LineSeries extremumsMonkey = new()
            {
                Title = "Extremum-Monkey",
                Color = OxyColors.Transparent,
                MarkerFill = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                Tag = "ExtremumsMonkey",
                XAxisKey = "X",
                YAxisKey = "Y"
            };
            LineSeries extremumsLine = new()
            {
                Title = "Extremums",
                Color = OxyColors.Gray,
                StrokeThickness = 1,
                Tag = "Extremum",
                XAxisKey = "X",
                YAxisKey = "Y"
            };
            LineSeries currentPrice = new()
            {
                Title = "CurrentPrice",
                Color = OxyColors.Transparent,
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                Tag = "CurrentPrice",
                XAxisKey = "X",
                YAxisKey = "Y"
            };
            currentPrice.Points.Add(new DataPoint(0, 0));

            plot.Axes.Add(axisX);
            plot.Axes.Add(axisY);
            plot.Series.Add(extremumsLine);
            plot.Series.Add(extremumsN);
            plot.Series.Add(extremumsMonkey);
            plot.Series.Add(currentPrice);

            OxyChart.Model = plot;
            OxyChart.InvalidatePlot(true);
        }
        private void PrintRansac(Ransac ransac)
        {
            LineSeries reg = new()
            {
                Color = ransac.Slope > 0 ? OxyColors.Lime : OxyColors.Red,
                StrokeThickness = 3,
                XAxisKey = "X",
                YAxisKey = "Y",
                Tag = "ransac"
            };
            LineSeries sigma = new()
            {
                Color = ransac.Slope > 0 ? OxyColors.Lime : OxyColors.Red,
                StrokeThickness = 2,
                LineStyle = LineStyle.Dot,
                XAxisKey = "X",
                YAxisKey = "Y",
                Tag = "ransac"
            };

            double y1 = ransac.Transform(ransac.FirstIndexTick);
            double y2 = ransac.Transform(ransac.EndIndexTick - 1);

            reg.Points.Add(new DataPoint(ransac.FirstIndexTick, y1));
            reg.Points.Add(new DataPoint(ransac.EndIndexTick - 1, y2));
            sigma.Points.Add(new DataPoint(ransac.FirstIndexTick, ransac.Slope > 0 ? y1 - ransac.Sigma : y1 + ransac.Sigma));
            sigma.Points.Add(new DataPoint(ransac.EndIndexTick - 1, ransac.Slope > 0 ? y2 - ransac.Sigma : y2 + ransac.Sigma));

            OxyChart.Model.Series.Add(reg);
            OxyChart.Model.Series.Add(sigma);
        }
        private void PrintRansacs(List<Ransac> ransacs)
        {
            ClearRansacs();

            for (int i = 0; i < ransacs.Count; i++)
                PrintRansac(ransacs[i]);
        }
        private void ClearRansacs()
        {
            for (int i = OxyChart.Model.Series.Count - 1; i >= 0; i--)
            {
                if ((string)OxyChart.Model.Series[i].Tag == "ransac")
                    OxyChart.Model.Series.RemoveAt(i);
            }
        }
        private void TextToTextBox(TextBox tb, string text)
        {
            if (tb.InvokeRequired)
            {
                TextBoxTextDelegate d = new(TextToTextBox);
                tb.Invoke(d, new object[] { tb, text });
            }
            else tb.Text = text;
        }

        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
        //    InitializationModel();

        //    await Task.Run(() =>
        //    {
        //        monkeyNFilter = new(100);
        //        vertexes = new();
        //        ransacHystory = new(vertexes, TypeSigma.СonfidenceInterval);
        //        monkeyNFilter.NewVertex += vertexes.OnNewVertex;
        //        monkeyNFilter.NewVertex += MonkeyNFilter_NewVertex;
        //        ransacHystory.RebuildRansac += RansacHystory_RebuildRansac;
        //        ransacHystory.NewRansac += RansacHystory_NewRansac;

        //        int current = 0;
        //        for (int i = 0; i < 200; i++)
        //        {
        //            int rand = new Random().Next(-1000, i * 1000);
        //            current += rand;
        //            //((LineSeries)OxyChart.ActualModel.Series[0]).Points.Add(new DataPoint(i, rand));
        //            //Connector_NewPrice(rand);
        //            //OxyChart.ActualModel.InvalidatePlot(true);
        //            monkeyNFilter.OnNewTick(new Tick(0, 0, rand));
        //            Thread.Sleep(100);
        //            //Task.Delay(100);
        //        }
        //    });
        }
    }
}
