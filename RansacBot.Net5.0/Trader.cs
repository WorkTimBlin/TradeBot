namespace RansacBot.Net5._0
{
    internal static class Trader
    {
        #region Общие настройки

        public static Tool? Tool { get; private set; }

        #endregion

        #region Настройки стратегии 

        public static int N { get; private set; }


        #endregion


        public static void InitTrader(Tool tool)
        {
            if (tool != null && tool.PriceStep != 0 && tool.Step != 0)
                Tool = tool;
            else
                Tool = null;
        }
        public static void SetN(int n)
        {
            N = n;
        }
    }
}
