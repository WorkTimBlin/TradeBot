using NLog;

namespace RansacBot
{
    internal static class LOGGER
    {
        public delegate void MessageHandler(string message);
        public static event MessageHandler NewMessage;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Обычные сообщения с информацией о работе программы.
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
            NewMessage?.Invoke(message);
            logger.Trace(message);
        }
        /// <summary>
        /// Логи для дебага.
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            NewMessage?.Invoke(message);
            logger.Debug(message);
        }
        /// <summary>
        /// Сообщения с информацией, не связанные с работой программы.
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            NewMessage?.Invoke(message);
            logger.Info(message);
        }
        /// <summary>
        /// Предупреждения - 
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            NewMessage?.Invoke(message);
            logger.Warn(message);
        }
        /// <summary>
        /// Ошибки не фатальные.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            NewMessage?.Invoke(message);
            logger.Error(message);
        }
        /// <summary>
        /// Фатальные ошибки.
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            NewMessage?.Invoke(message);
            logger.Fatal(message);
        }
    }
}
