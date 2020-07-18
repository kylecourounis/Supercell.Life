namespace Supercell.Life
{
    using System.Diagnostics;
    using System.Reflection;

    using Supercell.Life.Titan.Helpers;

    public static class Debugger
    {
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

            // System.Diagnostics.Debug.WriteLine($"{Prefix} {$"{Method.DeclaringType.Name}::{Method.Name}".Pad()} : {Message}");

            if (method == null)
            {
                System.Diagnostics.Debug.WriteLine($"{prefix} {"null::null".Pad()} : {message}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"{prefix} {$"{method.DeclaringType?.Name}::{method.Name.Replace("get_", string.Empty).Replace("set_", string.Empty)}".Pad()} : {message}");
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