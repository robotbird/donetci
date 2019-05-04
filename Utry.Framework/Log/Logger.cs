using System;
using log4net;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "logConfig.xml", Watch = true)]

namespace Utry.Framework.Log
{
   
    public enum LogType
    {
        DEBUG = 1,
        ERROR = 2,
        INFO = 3,
        WARN = 4
    }

    /// <summary>
    /// 日志记录工具类
    /// </summary>
    public class Logger
    {
        #region 全局声明
        //日志对象

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Logger));
        private static object lockHelper = new object();
        #endregion

        #region 方法
        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            log.Error(message);
        }
        public static void WriteLog(LogType type, object message, Exception exception)
        {
            switch (type)
            {
                case LogType.DEBUG:
                    log.Debug(message, exception);
                    break;
                case LogType.ERROR:
                    log.Error(message, exception);
                    break;
                case LogType.INFO:
                    log.Info(message, exception);
                    break;
                case LogType.WARN:
                    log.Warn(message, exception);
                    break;
                default:
                    break;
            }
        }

        public static void WriteLog(LogType type, object message)
        {
            switch (type)
            {
                case LogType.DEBUG:
                    log.Debug(message);
                    break;
                case LogType.ERROR:
                    log.Error(message);
                    break;
                case LogType.INFO:
                    log.Info(message);
                    break;
                case LogType.WARN:
                    log.Warn(message);
                    break;
                default:
                    break;
            }
        }

        #endregion

        public static log4net.ILog Log
        {
            get
            {
                if (log == null)
                    lock (lockHelper)
                        if (log == null)
                            log = log4net.LogManager.GetLogger(typeof(Logger));
                return log;
            }
        }

    }
}
