using RansacRealTime;

namespace RansacBot.Net5._0
{
    internal static class ToolObserver
    {
        public static Tool CurrentTool { get; private set; }
        public static RansacObserver Data { get; private set; }
        public static int N { get; private set; }
        public static double PercentCloseN1 { get; private set; }

        public static void Initialization(RansacObserver ransacObserver, Tool tool, int n, double percent)
        {
            CurrentTool = tool;
            Data = ransacObserver;
            N = n;
            PercentCloseN1 = percent;
        }
    }
}
