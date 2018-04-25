using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 写日志
    /// </summary>
    public delegate void WriteDebug(string message);
    public delegate void WriteInfo(string message);
    public delegate void WriteError(string message, System.Exception ex);
    public static class Log
    {
        public static WriteDebug d;
        public static WriteInfo i;
        public static WriteError e;
        private static string logDir { get; set; }
        private static string softName { get; set; }
        static Log()
        {
            #region---日志地址---
            string dirpath = System.Configuration.ConfigurationManager.AppSettings.Get("LogPath");
            if (string.IsNullOrEmpty(dirpath))
                dirpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            softName = System.Configuration.ConfigurationManager.AppSettings.Get("SoftName");
            if (string.IsNullOrEmpty(softName))
                softName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            logDir = System.IO.Path.Combine(dirpath, softName);
            try
            {
                if (!System.IO.Directory.Exists(logDir))
                {
                    System.IO.Directory.CreateDirectory(logDir);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            #endregion
            d = (string message) =>
            {
                writeLine(LogType.debug, message);
            };
            i = (string message) =>
            {
                writeLine(LogType.info, message);
            };
            e = (string message, System.Exception ex) =>
            {
                writeLine(LogType.error, message, ex);
            };
            System.AppDomain.CurrentDomain.ProcessExit += (object sender, System.EventArgs args) => { Dispose(); };                    
        }
        private static void Dispose() { }
        private static void writeLine(LogType logType, string message, System.Exception ex = null, string logPath = null)
        {
            string logStr = string.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string format = "line:{0};cols:{1};in {2}\r\n";
            if (ex != null) message += " 附加信息:" + ex.Message;
            sb.AppendFormat("时间:{0} 级别:{1},简要描述:{2}\r\n", System.DateTime.Now.ToLongTimeString(), logType, message);
            if (LogType.error == logType)
            {
                System.Diagnostics.StackTrace st = ex != null ? new System.Diagnostics.StackTrace(ex, true) : new System.Diagnostics.StackTrace(true);
                System.Diagnostics.StackFrame[] frames = st.GetFrames();
                for (int i = 0; i < frames.Length; i++) /* Ignore current StackTraceToString method...*/
                {
                    var currFrame = frames[i];
                    System.Reflection.MethodBase method = currFrame.GetMethod();
                    if (method == null) continue;
                    string name = method.DeclaringType != null ? method.DeclaringType.FullName : string.Empty;
                    //if (!name.StartsWith("JC.CRM")) continue;
                    if (method.ReflectedType == typeof(Log)) continue;

                    string fileName = null;
                    try
                    {
                        fileName = currFrame.GetFileName();
                    }
                    catch (System.NotSupportedException)
                    {
                        continue;
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }

                    sb.Append(string.Format("at {0}:{1}\r\n",
                        method.DeclaringType != null ? method.ReflectedType.FullName.Replace('+', '.') : string.Empty,
                        method.Name));
                    var line = currFrame.GetFileLineNumber();
                    if (0 < line && !string.IsNullOrEmpty(fileName))
                    {
                        var cols = currFrame.GetFileColumnNumber();
                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, format, new object[] { line, cols, fileName });
                    }
                }
            }
            sb.Append("\r\n");
            logStr = sb.ToString();
            try
            {
                var logFile = System.IO.Path.Combine(logDir, string.Format("{0}.log", System.DateTime.Now.ToString("yyyyMMdd")));
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(logFile))
                {
                    sw.BaseStream.Seek(0, System.IO.SeekOrigin.End);
                    sw.Write(logStr);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (System.Exception ex1)
            {
                System.Diagnostics.Debug.WriteLine(ex1.ToString());
            }

        }
    }
    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 信息日志
        /// </summary>
        info,
        /// <summary>
        /// 测试日志
        /// </summary>
        debug,
        /// <summary>
        /// 错误日志
        /// </summary>
        error
    }
}
