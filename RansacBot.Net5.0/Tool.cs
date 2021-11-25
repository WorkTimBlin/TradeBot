using QuikSharp.DataStructures;
using System;
using System.Globalization;

namespace RansacBot.Net5._0
{
    /// <summary>
    /// Торговый инструмент (бумага). Адаптирован строго под фьючерсы.
    /// </summary>
    internal class Tool
    {
        #region Свойства

        private static readonly Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

        /// <summary>
        /// Краткое наименование инструмента (бумаги)
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Код инструмента (бумаги)
        /// </summary>
        public string SecurityCode { get; private set; }
        /// <summary>
        /// Код класса инструмента (бумаги)
        /// </summary>
        public string ClassCode { get; private set; }
        /// <summary>
        /// Код клиента (номер счета)
        /// </summary>
        public string ClientCode { get; private set; }
        /// <summary>
        /// Счет клиента
        /// </summary>
        public string AccountID { get; private set; }
        /// <summary>
        /// Код фирмы
        /// </summary>
        public string FirmID { get; private set; }
        /// <summary>
        /// Точность цены (количество знаков после запятой)
        /// </summary>
        public int PriceAccuracy { get; private set; }
        /// <summary>
        /// Шаг цены
        /// </summary>
        public double Step { get; private set; }
        /// <summary>
        /// Стоимость шага цены
        /// </summary>
        public double PriceStep { get; private set; }
        /// <summary>
        /// Гарантийное обеспечение покупателя(только для срочного рынка)
        /// </summary>
        public double GOBuy { get; private set; }
        /// <summary>
        /// Гарантийное обеспечение продавца(только для срочного рынка)
        /// </summary>
        public double GOSell { get; private set; }

        #endregion

        /// <summary>
        /// Базовый конструктор. Принимает код класса инструмента (Фьючерсы: "SPBFUT") и тикер(SecCode) инструмента. (RTS например имеет тикер - RIZ1). <br/>
        /// А также ClientCode - это номер счета клиента.
        /// </summary>
        /// <param name="secCode"></param>
        /// <param name="classCode"></param>
        public Tool(string secCode, string clientCode, string accountId, string firmId = "SPBFUT", string classCode = "SPBFUT")
        {
            SecurityCode = secCode;
            ClientCode = clientCode;
            ClassCode = classCode;
            AccountID = accountId;
            FirmID = firmId;

            SetBaseParam();
            SetGOInfo();
        }

        /// <summary>
        /// Устанавливает базовые параметры инструмента.
        /// </summary>
        private void SetBaseParam()
        {
            if (ClassCode != null && ClassCode != "")
            {
                try
                {
                    SecurityInfo infoTool = Connector.quik.Class.GetSecurityInfo(ClassCode, SecurityCode).Result;
                    
                    if (infoTool != null)
                    {
                        Name = infoTool.Name;
                        Step = infoTool.MinPriceStep;
                        PriceAccuracy = infoTool.Scale;
                    }
                    else
                    {
                        LOGGER.Message("Tool.SetBaseParam(): Warning - Не удалось загрузить информацию об инструменте " + SecurityCode);
                    }
                }
                catch (Exception ex)
                {
                    LOGGER.Message("Tool.SetBaseParam(): Exception - " + ex.Message);
                }
            }
            else
            {
                LOGGER.Message("Tool.SetBaseParam(): Warning - Код класса '" + ClassCode + "' инструмента не обнаружен.");
            }
        }
        /// <summary>
        /// Устанавливает параметры ГО и стоимости шага цены. 
        /// </summary>
        private void SetGOInfo()
        {
            try
            {
                GOBuy = Convert.ToDouble(Connector.quik.Trading.GetParamEx(ClassCode, SecurityCode, ParamNames.BUYDEPO).Result.ParamValue.Replace('.', separator));
                GOSell = Convert.ToDouble(Connector.quik.Trading.GetParamEx(ClassCode, SecurityCode, ParamNames.SELLDEPO).Result.ParamValue.Replace('.', separator));
                PriceStep = Convert.ToDouble(Connector.quik.Trading.GetParamEx(ClassCode, SecurityCode, ParamNames.STEPPRICE).Result.ParamValue.Replace('.', separator));
            }
            catch (Exception ex)
            {
                LOGGER.Message("Tool.SetGOInfo(): Exception во время загрузки ГО инструмента: " + ex.Message);
            }
        }
    }
}
