using CitizenFX.Core;
using jkAnti.Client;
using System;

namespace jkAnti.Client
{
    public static class Logger
    {
        public static void Log(string msg, Logger.LogLevel level = Logger.LogLevel.NONE)
        {
            string prefix = "[jkAnti] ";
            switch (level)
            {
                case LogLevel.ERROR:
                    Debug.WriteLine(prefix + "^1[ERROR] " + msg + "^0");
                    break;
                case LogLevel.BLUE:
                    Debug.WriteLine("^5" + msg + "^0");
                    break;
                case LogLevel.INFO:
                    Debug.WriteLine(prefix + "^5[INFO] " + msg + "^0");
                    break;
                case LogLevel.NONE:
                    Console.WriteLine(msg);
                    break;
                case LogLevel.DEBUG:
                    if (ConfigManager.Config.DebugMode == true)
                    {
                        Debug.WriteLine(prefix + "[DEBUG] " + msg);
                    }
                    break;
                case LogLevel.WARNING:
                    Debug.WriteLine(prefix + "^8[WARNING] " + msg + "^0");
                    break;
            }
        }
        public enum LogLevel
        {
            NONE,
            INFO,
            BLUE,
            WARNING,
            DEBUG,
            ERROR,
        }
    }
}
