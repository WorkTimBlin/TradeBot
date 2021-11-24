namespace RansacBot.Net5._0
{
    internal static class LOGGER
    {
        public delegate void NewMessageHandler(string message);
        public static event NewMessageHandler NewMessage;

        public static void Message(string message)
        {
            NewMessage?.Invoke(message);
        }
    }
}
