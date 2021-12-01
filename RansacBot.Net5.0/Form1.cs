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
using System.IO;

namespace RansacBot.Net5._0
{
    public partial class FormMain : Form
    {
        delegate void TextBoxTextDelegate(TextBox tb, string text);
        private int currentLevel { get; set; }
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
            timerTest.Stop();
        }
        private void LoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new();
            formLogin.ShowDialog();

            try
            {
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

            //Connector.Subscribe(ToolObserver.CurrentTool.ClassCode, ToolObserver.CurrentTool.SecurityCode, ToolObserver.Data.MonkeyNFilter.OnNewTick);

            Connector.NewPrice = Connector_NewPrice;
            //ToolObserver.Data.MonkeyNFilter.NewVertex += ToolObserver.Data.Vertexes.OnNewVertex;
            //ToolObserver.Data.MonkeyNFilter.NewVertex += MonkeyNFilter_NewVertex;

            cbRansac.Items.AddRange(InstrumentObserver.ransacsObserver.vertexes.Hystories.Select(x => x.Type.ToString()).ToArray());
            cbRansac.SelectedIndex = 0;
            nmcLevelRansac.Value = InstrumentObserver.ransacsObserver.vertexes.Hystories[cbRansac.SelectedIndex].MaxLevel + 1;

            cbRansac.Visible = true;
            nmcLevelRansac.Visible = true;
            OnToolStripMenuItem.Enabled = false;
            LOGGER.Trace("Восстание машин активировано!");
            timer.Start();
        }
        private void CbRansac_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentHystory = cbRansac.SelectedIndex;
            var level = InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].Levels.Count == 0 ? null : InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].Levels[^1];
           
            for (int i = 0; i < InstrumentObserver.ransacsObserver.vertexes.Hystories.Count; i++)
            {
                InstrumentObserver.ransacsObserver.vertexes.Hystories[i].RebuildRansac -= RansacHystory_RebuildRansac;
                InstrumentObserver.ransacsObserver.vertexes.Hystories[i].NewRansac -= RansacHystory_NewRansac;
            }

            InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].RebuildRansac += RansacHystory_RebuildRansac;
            InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].NewRansac += RansacHystory_NewRansac;

            NmcLevelRansac_ValueChanged(sender, e);
            nmcLevelRansac.Maximum = InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].MaxLevel + 1;
        }
        private void NmcLevelRansac_ValueChanged(object sender, EventArgs e)
        {
            currentLevel = (int)nmcLevelRansac.Value - 2;
            ClearRansacs();

            if (currentLevel < InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].Levels.Count)
            {
                PrintRansacs(InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].Levels[currentLevel].GetRansacs());
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

            UpdateCurrentParams();
        }

        #endregion

        #region Обработка событий

        private void MonkeyNFilter_NewVertex(Tick tick, VertexType vertexType)
        {
            PrintVertex(tick, vertexType);
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
            if (InstrumentObserver.instrument != null)
            {
                TextToTextBox(tbClientCode, InstrumentObserver.instrument.clientCode.ToString());
                TextToTextBox(tbAccountID, InstrumentObserver.instrument.accountID.ToString());
                TextToTextBox(tbClassCode, InstrumentObserver.instrument.classCode.ToString());
                TextToTextBox(tbFirmID, InstrumentObserver.instrument.firmID.ToString());
                TextToTextBox(tbSecCode, InstrumentObserver.instrument.securityCode.ToString());
                TextToTextBox(tbShortName, InstrumentObserver.instrument.name.ToString());
                TextToTextBox(tbStep, InstrumentObserver.instrument.step.ToString());
                TextToTextBox(tbStepPrice, InstrumentObserver.instrument.stepPrice.ToString());
                TextToTextBox(tbGoBuy, InstrumentObserver.instrument.initialMarginBuy.ToString());
                TextToTextBox(tbGoSell, InstrumentObserver.instrument.initialMarginSell.ToString());
            }
            else ClearInterface();
        }
        private void UpdateCurrentParams()
        {
            if (InstrumentObserver.instrument != null)
            {
                TextToTextBox(tbAvailableMax, Connector.quik.Trading.CalcBuySell(InstrumentObserver.instrument.classCode, InstrumentObserver.instrument.securityCode, InstrumentObserver.instrument.clientCode, InstrumentObserver.instrument.accountID, 0, true, true).Result.Qty + " | " +
                    Connector.quik.Trading.CalcBuySell(InstrumentObserver.instrument.classCode, InstrumentObserver.instrument.securityCode, InstrumentObserver.instrument.clientCode, InstrumentObserver.instrument.accountID, 0, false, true).Result.Qty);
                TextToTextBox(tbCurrentPos, "0 | 0");
                
                PortfolioInfo portfolio = Connector.quik.Trading.GetPortfolioInfo(InstrumentObserver.instrument.firmID, InstrumentObserver.instrument.clientCode).Result;

                if (portfolio != null)
                {
                    TextToTextBox(tbVarMargin, Convert.ToDouble(portfolio.VarMargin.Replace('.', ',')).ToString());
                    TextToTextBox(tbBalanceNoMargin, Convert.ToDouble(portfolio.AllAssets.Replace('.', ',')).ToString());
                    TextToTextBox(tbBalance, Convert.ToDouble(portfolio.TotalMoneyBal.Replace('.', ',')).ToString());
                    TextToTextBox(tbBlock, (Convert.ToDouble(portfolio.TotalMoneyBal.Replace('.',',')) - Convert.ToDouble(portfolio.LimNonMargin.Replace('.', ','))).ToString());
                    TextToTextBox(tbAvailableFunds, Convert.ToDouble(portfolio.LimNonMargin.Replace('.', ',')).ToString());
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
                Title = "MonkeyN - " + InstrumentObserver.N,
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
        private void PrintVertex(Tick tick, VertexType vertexType)
        {
            ((LineSeries)OxyChart.Model.Series[0]).Points.Add(new DataPoint(tick.VERTEXINDEX, tick.PRICE));

            if (vertexType == VertexType.Monkey)
                ((LineSeries)OxyChart.Model.Series[2]).Points.Add(new DataPoint(tick.VERTEXINDEX, tick.PRICE));
            else
                ((LineSeries)OxyChart.Model.Series[1]).Points.Add(new DataPoint(tick.VERTEXINDEX, tick.PRICE));

            if (currentLevel < InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].Levels.Count && InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].Levels[currentLevel].IsBuilding)
                RansacHystory_RebuildRansac(InstrumentObserver.ransacsObserver.vertexes.Hystories[currentHystory].Levels[currentLevel].GetRansacs().Last(), currentLevel);
        }
        private void PrintVertexes()
        {
            ClearVertexes();

            for (int i = 0; i < InstrumentObserver.ransacsObserver.vertexes.VertexList.Count; i++)
                PrintVertex(InstrumentObserver.ransacsObserver.vertexes.VertexList[i], VertexType.High);
        }
        private void ClearVertexes()
        {
            ((LineSeries)OxyChart.Model.Series[0]).Points.Clear();
            ((LineSeries)OxyChart.Model.Series[1]).Points.Clear();
            ((LineSeries)OxyChart.Model.Series[2]).Points.Clear();
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
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                InstrumentObserver.Save("SAVE", true);

            }
            catch (Exception ex)
            {
                LOGGER.Trace("Ошибка при сохранении: " + ex.Message);
            }
        }


        Queue<Tick> ticksTest;
        private void HystoryTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ticksTest = new();
            LOGGER.Trace("loaded");
            FormTestLogin formTestLogin = new();
            formTestLogin.ShowDialog();

            if (formTestLogin.DialogResult == DialogResult.OK)
            {
                UpdateStaticParams();
                UpdateCurrentParams();
                LoginToolStripMenuItem.Enabled = false;
                HystoryTestToolStripMenuItem.Enabled = false;
                OnToolStripMenuItem.Enabled = false;

                InitializationModel();

                InstrumentObserver.ransacsObserver.monkeyNFilter.NewVertex += MonkeyNFilter_NewVertex;


                using StreamReader reader = new(@"C:\Users\ir2\Desktop\Программы\DATA\TICKS\RTS.F\3.txt");
                //using StreamReader reader = new(@"F:\tim\monte-carlo\DATA\TICKS\RTS.F\3.txt");
                reader.ReadLine();
                LOGGER.Trace("start loading...");

                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (Convert.ToInt32(data[0]) > InstrumentObserver.ransacsObserver.vertexes.VertexList.Last().ID)
                        ticksTest.Enqueue(new Tick(Convert.ToInt32(data[0]), 0, Convert.ToInt32(data[2])));
                }

                cbRansac.Items.AddRange(InstrumentObserver.ransacsObserver.vertexes.Hystories.Select(x => x.Type.ToString()).ToArray());
                cbRansac.SelectedIndex = 0;
                nmcLevelRansac.Value = InstrumentObserver.ransacsObserver.vertexes.Hystories[cbRansac.SelectedIndex].MaxLevel + 1;
                cbRansac.Visible = true;
                nmcLevelRansac.Visible = true;
                OnToolStripMenuItem.Enabled = false;
                LOGGER.Trace("Подключение выполнено");
                timer.Start();
                PrintVertexes();
                timerTest.Start();
            }
            else
            {
                LOGGER.Trace("Подключение отменено!");
            }
        }
        private void timerTest_Tick(object sender, EventArgs e)
        {
            if (ticksTest.Count == 0)
            {
                timerTest.Stop();
                LOGGER.Trace("Тики закончились");
                return;
            }

            for (int i = 0; i < 20; i++)
            {
                Tick tick = ticksTest.Dequeue();
                InstrumentObserver.ransacsObserver.monkeyNFilter.OnNewTick(tick);
                Connector_NewPrice(tick.PRICE);
            }

            LOGGER.Trace("Осталось тиков: " + ticksTest.Count);
        }
    }
}
