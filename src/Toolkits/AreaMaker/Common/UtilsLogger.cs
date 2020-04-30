using System;

namespace AreaMaker.Common
{
    public class UtilsLogger
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message"></param>
        public static void LogMessage(string message)
        {
            defaultLogAction.Invoke(message);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        public static void LogMessage(string category, string message)
        {
            defaultLogAction.Invoke(string.Format("[{0}] {1}", category, message));
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void LogMessage(Type type, string message)
        {
            defaultLogAction.Invoke(string.Format("[{0}] {1}", type.Name, message));
        }
        
        /// <summary>
        /// 重新设置日志打印方式
        /// </summary>
        /// <param name="action"></param>
        public static void SetLogFunc(Action<string> action)
        {
            if (action != null)
            {
                defaultLogAction = action;
            }
        }

        public static string Prefix { get; set; }

        public UtilsLogger()
        {
            Prefix = "AreaMaker => ";
        }

        private static Action<string> defaultLogAction = new Action<string>(TraceMessage);
        private static void TraceMessage(string message)
        {
            System.Diagnostics.Trace.WriteLine(Prefix + message);
        }
    }
}
