using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using QuikSharp.DataStructures;
using RansacRealTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace RansacBot.Net5._0
{
    public partial class FormMain : Form
    {
        delegate void TextBoxTextDelegate(TextBox tb, string text);
        private MonkeyNFilter monkeyNFilter;
        private Vertexes vertexes;
        private RansacHystory ransacHystory;

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
            FormLogin formLogin = new();
            formLogin.ShowDialog();

            if (formLogin.DialogResult == DialogResult.OK)
            {
                UpdateStaticParams();
                UpdateCurrentParams();
                LoginToolStripMenuItem.Enabled = false;
                OnToolStripMenuItem.Enabled = true;
            }
        }
        private void OnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Trader.Tool != null)
            {
                InitializationModel();
                monkeyNFilter = new(Trader.N);
                vertexes = new();
                ransacHystory = new(vertexes, TypeSigma.СonfidenceInterval, 3, 90.0);

                Connector.NewPrice = Connector_NewPrice;
                Connector.Subscribe(Trader.Tool.ClassCode, Trader.Tool.SecurityCode, monkeyNFilter.OnNewTick);


                monkeyNFilter.NewVertex += vertexes.OnNewVertex;
                monkeyNFilter.NewVertex += MonkeyNFilter_NewVertex;
                ransacHystory.RebuildRansac += RansacHystory_RebuildRansac;
                ransacHystory.NewRansac += RansacHystory_NewRansac;

                OnToolStripMenuItem.Enabled = false;
                LOGGER.Message("Подключение прошло успешно!");
                timer.Start();
            }
            else
            {

            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (OxyChart.InvokeRequired)
            {
                OxyChart.Invoke(new System.Action(() => { OxyChart.InvalidatePlot(true); }));

                LOGGER.Message("Необычная отрисовка");
            }
            else
            {
                OxyChart.InvalidatePlot(true);
                //LOGGER.Message("Просто отрисовка");
            }

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

            LOGGER.Message("Найдена новая вершина: " + tick.PRICE + " | Индекс: " + tick.VERTEXINDEX);
        }
        private void RansacHystory_NewRansac(Ransac ransac, int level)
        {
            if (level != 0)
                return;

            while ((OxyChart.Model.Series.Count - 4) > 20)
            {
                OxyChart.Model.Series.RemoveAt(4);
                OxyChart.Model.Series.RemoveAt(4);
            }

            PrintRansac(ransac);
            LOGGER.Message("Найден новый ранзак: " + (ransac.EndIndexTick - 1));
        }
        private void RansacHystory_RebuildRansac(Ransac ransac, int level)
        {
            if (level != 0)
                return;

            if (OxyChart.Model.Series.Count > 4)
            {
                OxyChart.Model.Series.RemoveAt(OxyChart.Model.Series.Count - 1);
                OxyChart.Model.Series.RemoveAt(OxyChart.Model.Series.Count - 1);
            }

            PrintRansac(ransac);
            LOGGER.Message("Текущий ранзак перестроен: " + (ransac.EndIndexTick - 1));
        }
        private void Connector_NewPrice(double price)
        {
            ((LineSeries)OxyChart.Model.Series[3]).Points[0] = new DataPoint(((LineSeries)OxyChart.Model.Series[0]).Points.Count, price);
        }
        private void Logger_NewMessage(string message)
        {
            AddTextLog(message);
            //statusStrip.Items["toolStripStatus"].Text = message;
        }

        #endregion

        #region Вспомогательные функции

        private void UpdateStaticParams()
        {
            if (Trader.Tool != null)
            {
                TextToTextBox(tbClientCode, Trader.Tool.ClientCode.ToString());
                TextToTextBox(tbAccountID, Trader.Tool.AccountID.ToString());
                TextToTextBox(tbClassCode, Trader.Tool.ClassCode.ToString());
                TextToTextBox(tbFirmID, Trader.Tool.FirmID.ToString());
                TextToTextBox(tbSecCode, Trader.Tool.SecurityCode.ToString());
                TextToTextBox(tbShortName, Trader.Tool.Name.ToString());
                TextToTextBox(tbStep, Trader.Tool.Step.ToString());
                TextToTextBox(tbStepPrice, Trader.Tool.PriceStep.ToString());
                TextToTextBox(tbGoBuy, Trader.Tool.GOBuy.ToString());
                TextToTextBox(tbGoSell, Trader.Tool.GOSell.ToString());
            }
            else ClearInterface();
        }
        private void UpdateCurrentParams()
        {
            if (Trader.Tool != null)
            {
                TextToTextBox(tbAvailableMax, Connector.quik.Trading.CalcBuySell(Trader.Tool.ClassCode, Trader.Tool.SecurityCode, Trader.Tool.ClientCode, Trader.Tool.AccountID, 0, true, true).Result.Qty + " | " +
                    Connector.quik.Trading.CalcBuySell(Trader.Tool.ClassCode, Trader.Tool.SecurityCode, Trader.Tool.ClientCode, Trader.Tool.AccountID, 0, false, true).Result.Qty);
                TextToTextBox(tbCurrentPos, "+3 | -2");
                FuturesClientHolding futuresClient = Connector.quik.Trading.GetFuturesHolding(Trader.Tool.FirmID, Trader.Tool.AccountID, Trader.Tool.SecurityCode, 1).Result;

                if (futuresClient != null)
                {
                    TextToTextBox(tbVarMargin, futuresClient.varMargin.ToString());
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
                Title = "MonkeyN - " + Trader.N,
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
                YAxisKey = "Y"
            };
            LineSeries sigma = new()
            {
                Color = ransac.Slope > 0 ? OxyColors.Lime : OxyColors.Red,
                StrokeThickness = 2,
                LineStyle = LineStyle.Dot,
                XAxisKey = "X",
                YAxisKey = "Y"
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
        private void TextToTextBox(TextBox tb, string text)
        {
            if (tb.InvokeRequired)
            {
                TextBoxTextDelegate d = new(TextToTextBox);
                tb.Invoke(d, new object[] { tb, text });
            }
            else tb.Text = text;
        }
        private void AddTextLog(string text)
        {
            try
            {
                if (cbStatus.InvokeRequired)
                {
                    cbStatus.Invoke(new System.Action(() =>
                    {
                        cbStatus.Items.Add(text);
                        cbStatus.SelectedIndex = cbStatus.Items.Count - 1;
                    }));
                }
                else
                {
                    cbStatus.Items.Add(text);
                    cbStatus.SelectedIndex = cbStatus.Items.Count - 1;
                }
            }
            catch
            {

            }
        }

        #endregion

        private async void button1_Click_1(object sender, EventArgs e)
        {
            InitializationModel();

            await Task.Run(() =>
            {
                monkeyNFilter = new(100);
                vertexes = new();
                ransacHystory = new(vertexes, TypeSigma.СonfidenceInterval);
                monkeyNFilter.NewVertex += vertexes.OnNewVertex;
                monkeyNFilter.NewVertex += MonkeyNFilter_NewVertex;
                ransacHystory.RebuildRansac += RansacHystory_RebuildRansac;
                ransacHystory.NewRansac += RansacHystory_NewRansac;

                int current = 0;
                for (int i = 0; i < 200; i++)
                {
                    int rand = new Random().Next(-1000, i * 1000);
                    current += rand;
                    //((LineSeries)OxyChart.ActualModel.Series[0]).Points.Add(new DataPoint(i, rand));
                    //Connector_NewPrice(rand);
                    //OxyChart.ActualModel.InvalidatePlot(true);
                    monkeyNFilter.OnNewTick(new Tick(0, 0, rand));
                    Thread.Sleep(100);
                    //Task.Delay(100);
                }
            });
        }


    }
}
