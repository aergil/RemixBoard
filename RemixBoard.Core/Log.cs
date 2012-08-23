using System;
using log4net;

namespace RemixBoard.Core
{
    public static class Log
    {
        public static ILog GeneralLogger {
            get {
                if (exceptionLogger == null) {
                    log4net.Config.XmlConfigurator.Configure();
                    exceptionLogger = LogManager.GetLogger("GeneralLogger");
                }

                return exceptionLogger;
            }
            set { exceptionLogger = value; }
        }

        private static ILog exceptionLogger;

        public static void Info(object sender, string message) {
            if (Logged != null)
                Logged(sender, new LogEventArgs(message));

            GeneralLogger.Info(message);
        }

        public static void Error(object sender, string message, Exception exception) {
            if (Logged != null)
                Logged(sender, new LogEventArgs(message));

            GeneralLogger.Error(message, exception);
        }

        public static event LogEventHandler Logged;
    }

    public delegate void LogEventHandler(object sender, LogEventArgs args);

    public class LogEventArgs
    {
        public string Message { get; set; }

        public LogEventArgs(string message) {
            Message = message;
        }
    }
}