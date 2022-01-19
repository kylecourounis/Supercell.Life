namespace Supercell.Life
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    using Supercell.Life.Titan.Helpers;

    public static class Debugger
    {
        private static FileInfo File;

        private static readonly object FileLock = new object();

        /// <summary>
        /// Gets a value indicating whether this <see cref="Debugger"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Debugger"/> class.
        /// </summary>
        public static void Initialize()
        {
            if (Debugger.Initialized)
            {
                return;
            }

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            Debugger.File = new FileInfo($"Logs/{DateTime.Now.ToString("s").Replace(":", ".")}.log");
            Debugger.File.Create().Close();

            Debugger.Initialized = true;
        }

        /// <summary>
        /// Logs the specified informative message.
        /// </summary>
        public static void Info(object message, MethodBase method = null)
        {
            if (method == null)
            {
                method = new StackFrame(1).GetMethod();
            }

            Debugger.Log(message, method, LogType.Info);
        }
        
        /// <summary>
        /// Logs the specified debug message.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Debug(object message, MethodBase method = null)
        {
            if (method == null)
            {
                method = new StackFrame(1).GetMethod();
            }

            Debugger.Log(message, method, LogType.Debug);
        }

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        [Conditional("DEBUG")]
        public static void Warning(object message, MethodBase method = null)
        {
            if (method == null)
            {
                method = new StackFrame(1).GetMethod();
            }

            Debugger.Log(message, method, LogType.Warning);
        }
        
        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        public static void Error(object message, MethodBase method = null)
        {
            if (method == null)
            {
                method = new StackFrame(1).GetMethod();
            }

            Debugger.Log(message, method, LogType.Error);
        }
        
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        private static void Log(object message, MethodBase method, LogType logType)
        {
            string prefix = string.Empty;

            switch (logType)
            {
                case LogType.Info:
                {
                    prefix = "[ INFO  ]";
                    break;
                }
                default:
                {
                    prefix = $"[{logType.ToString().ToUpper().Pad(7)}]";
                    break;
                }
            }

            if (method == null)
            {
                System.Diagnostics.Debug.WriteLine($"{prefix} {"null::null".Pad()} : {message}");
            }
            else
            {
                string msg = $"{prefix} {$"{method.DeclaringType?.Name}::{method.Name.Replace("get_", string.Empty).Replace("set_", string.Empty)}".Pad()} : {message}";
                
                System.Diagnostics.Debug.WriteLine(msg);

                lock (Debugger.FileLock)
                {
                    Debugger.File.AppendAllText(msg + Environment.NewLine);
                }
            }
        }

        private enum LogType
        {
            Info,
            Debug,
            Warning,
            Error
        }
    }
}