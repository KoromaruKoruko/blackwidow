using System;
using System.IO;

namespace BDLib.Text
{
    public static class Logger
    {
        private static StreamWriter _LOG;
        private static bool _LogActive;
        private static LogType _LogLevel;
        private static int _TimeLength = 0;
        private static bool _OutputToConsole;

        /// <summary>
        /// initulizes the logger for use
        /// </summary>
        /// <param name="LogName">name of log file</param>
        /// <param name="LogLevel">severity to log at</param>
        /// <param name="OutputToConsole">if i should output to the STD:OUT as well</param>
        /// <param name="OutputFolder">Output Folder for the log file</param>
        public static void INIT(string LogName, LogType LogLevel,bool OutputToConsole = false, string OutputFolder = "")
        {
            if (!StringHelpers.IsWhiteSpaceOrNull(OutputFolder) && !Directory.Exists(OutputFolder))
                Directory.CreateDirectory(OutputFolder);

            _LOG = new StreamWriter(new FileStream($"{OutputFolder}//{DateTime.Today.ToShortDateString()}-{DateTime.Now.ToShortTimeString().Replace(":",".")}-{LogName}.BDLog", FileMode.Create));
            _LogLevel = LogLevel;
            _LogActive = true;
            _OutputToConsole = OutputToConsole;
        }
        /// <summary>
        /// ends the logging secion and uninitulizes the logger
        /// </summary>
        public static void EndLog()
        {
            _LogActive = false;
            _LOG.Close();
            _LogLevel = 0;
            _TimeLength = 0;
        }

        /// <summary>
        /// logs the message
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="Type">severity</param>
        public static void Log(string message, LogType Type)
        {
            if (FORMAT_Verify_LogType(Type) && _LogActive)
            {
                string m = $"{FORMAT_LogType(Type)}{FORMAT_Time()}{message}";
                if (_OutputToConsole)
                    Console.WriteLine(m);
                _LOG.WriteLine(m);
            }
        }
        /// <summary>
        /// logs the message with some sublines
        /// sublines are for extra debugging info
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="SubLines">the extra info</param>
        /// <param name="Type">severity</param>
        public static void LogWithSubLines(string message, string[] SubLines, LogType Type)
        {
            if(FORMAT_Verify_LogType(Type) && _LogActive)
            {
                string m = $"{FORMAT_LogType(Type)}{FORMAT_Time()}{message}";
                if (_OutputToConsole)
                    Console.WriteLine(m);
                _LOG.WriteLine(m);
                for(int x = 0; x < SubLines.Length; x++)
                {
                    m = $"        +{SubLines[x]}";
                    if (_OutputToConsole)
                        Console.WriteLine(m);
                    _LOG.WriteLine($"        +{SubLines[x]}");
                }
            }
        }

        private static string FORMAT_Time()
        {
            string x = DateTime.Now.ToShortTimeString();
            if (x.Length > _TimeLength)
                _TimeLength = x.Length;
            else
                for(int y = x.Length;  y < _TimeLength; y++)
                {
                    x += " ";
                }

            x += ":";
            return x;
        }
        private static string FORMAT_LogType(LogType Type)
        {
            switch(Type)
            {
                case (LogType.Info):
                    return "INFO    #";
                case (LogType.Warning):
                    return "WARNING #";
                case (LogType.Error):
                    return "ERROR   #";

                default:
                    throw new IndexOutOfRangeException("wow you fucked the code up bad, if you got this");
            }
        }
        private static bool FORMAT_Verify_LogType(LogType Type)
        {
            if (Type >= _LogLevel)
                return true;
            else
                return false;
        }
    }
    public enum LogType
    {
        Info = 0,
        Warning = 1,
        Error = 2,
    }
}
