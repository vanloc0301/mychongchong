namespace SkyMap.Net.Core
{
    using log4net;
    using log4net.Config;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public static class LoggingService
    {
        private static readonly ILog innerlog = LogManager.GetLogger(typeof(LoggingService));
        public static LogEventHandler LogHandler;
        private static Dictionary<Type, ILog> Logs = new Dictionary<Type, ILog>();

        static LoggingService()
        {
            try
            {
                IList<string> list = FileUtility.SearchDirectory(Path.Combine(Environment.CurrentDirectory, "bin"), "*log.txt2*", false, true);
                foreach (string str in list)
                {
                    File.Delete(str);
                }
            }
            catch
            {
            }
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                if (!ConfigureAndWatch(entryAssembly.GetName().Name + "log4net.config"))
                {
                    ConfigureAndWatch("log4net.config");
                }
            }
            else
            {
                ConfigureAndWatch("log4net.config");
            }
        }

        private static bool ConfigureAndWatch(string logFileName)
        {
            string str = Path.Combine(PropertyService.ConfigDirectory, logFileName);
            Console.WriteLine(str);
            if (File.Exists(str))
            {
                XmlConfigurator.ConfigureAndWatch(new FileInfo(str));
            }
            else
            {
                str = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), logFileName);
                if (File.Exists(str))
                {
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(str));
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static void Debug(string message)
        {
            log.Debug(message);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Debug, null);
        }

        public static void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(format, args);
            RaiseLogEvent(string.Format(format, args), SkyMap.Net.Core.LogLevel.Debug, null);
        }

        public static void Error(Exception exception)
        {
            log.Error(exception);
            RaiseLogEvent(exception.Message, SkyMap.Net.Core.LogLevel.Error, exception);
        }

        public static void Error(string message)
        {
            log.Error(message);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Error, null);
        }

        public static void Error(string message, Exception exception)
        {
            log.Error(message, exception);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Error, exception);
        }

        public static void Error(Exception exc, string message, params object[] args)
        {
            log.Error(string.Format(message, args), exc);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Error, exc);
        }

        public static void ErrorFormatted(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
            RaiseLogEvent(string.Format(format, args), SkyMap.Net.Core.LogLevel.Error, null);
        }

        public static void Fatal(string message)
        {
            log.Fatal(message);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Fatal, null);
        }

        public static void Fatal(string message, Exception exception)
        {
            log.Fatal(message, exception);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Fatal, exception);
        }

        public static void FatalFormatted(string format, params object[] args)
        {
            log.FatalFormat(format, args);
            RaiseLogEvent(string.Format(format, args), SkyMap.Net.Core.LogLevel.Fatal, null);
        }

        public static void Info(string message)
        {
            log.Info(message);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Info, null);
        }

        public static void InfoFormatted(string format, params object[] args)
        {
            log.InfoFormat(format, args);
            RaiseLogEvent(string.Format(format, args), SkyMap.Net.Core.LogLevel.Info, null);
        }

        private static void RaiseLogEvent(string msg, SkyMap.Net.Core.LogLevel level, Exception ex)
        {
            string sender = "?source?";
            string sourceMember = "?member?";
            try
            {
                if (LogHandler == null)
                {
                    return;
                }
                StackFrame frame = new StackFrame(2, true);
                if (frame != null)
                {
                    if (frame.GetMethod().DeclaringType != null)
                    {
                        sender = frame.GetMethod().DeclaringType.Name;
                    }
                    if (frame.GetMethod() != null)
                    {
                        sourceMember = frame.GetMethod().Name + "():" + frame.GetFileLineNumber();
                    }
                }
            }
            catch
            {
            }
            try
            {
                if (LogHandler != null)
                {
                    LogHandler(sender, new LogEventArgs(sender, sourceMember, msg, level, ex));
                }
            }
            catch
            {
            }
        }

        public static void Warn(Exception exception)
        {
            log.Warn(exception);
            RaiseLogEvent(exception.Message, SkyMap.Net.Core.LogLevel.Warn, exception);
        }

        public static void Warn(string message)
        {
            log.Warn(message);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Warn, null);
        }

        public static void Warn(string message, Exception exception)
        {
            log.Warn(message, exception);
            RaiseLogEvent(message, SkyMap.Net.Core.LogLevel.Warn, exception);
        }

        public static void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(format, args);
            RaiseLogEvent(string.Format(format, args), SkyMap.Net.Core.LogLevel.Warn, null);
        }

        public static bool IsDebugEnabled
        {
            get
            {
                return log.IsDebugEnabled;
            }
        }

        public static bool IsErrorEnabled
        {
            get
            {
                return log.IsErrorEnabled;
            }
        }

        public static bool IsFatalEnabled
        {
            get
            {
                return log.IsFatalEnabled;
            }
        }

        public static bool IsInfoEnabled
        {
            get
            {
                return log.IsInfoEnabled;
            }
        }

        public static bool IsWarnEnabled
        {
            get
            {
                return log.IsWarnEnabled;
            }
        }

        private static ILog log
        {
            get
            {
                ILog innerlog;
                try
                {
                    Type reflectedType = new StackTrace().GetFrame(2).GetMethod().ReflectedType;
                    lock (Logs)
                    {
                        if (Logs.ContainsKey(reflectedType))
                        {
                            return Logs[reflectedType];
                        }
                        ILog logger = LogManager.GetLogger(reflectedType);
                        Logs.Add(reflectedType, logger);
                        innerlog = logger;
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.innerlog.Error(exception);
                    innerlog = LoggingService.innerlog;
                }
                return innerlog;
            }
        }
    }
}

