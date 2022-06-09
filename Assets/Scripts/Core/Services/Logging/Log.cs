namespace Core.Services.Logging
{
    /// <summary>
    /// Logging for the Unity client
    /// </summary>
    public static class Log
    {
        private static ILogTarget _target;

        /// <summary>
        /// Sets an ILogTarget implementation
        /// </summary>
        /// <param name="target"></param>
        public static void SetLogTarget(ILogTarget target)
        {
            _target = target;
        }


        /// <summary>
        /// Logs a debug level message.
        /// </summary>
        /// <param name="caller">The calling object or null.</param>
        /// <param name="message">The object to log.</param>
        /// <param name="replacements">String replacements.</param>
        public static void Debug(object message, params object[] replacements)
        {
            Out(
                LogLevel.Debug,
                message,
                replacements);
        }

        /// <summary>
        /// Logs an info level message.
        /// </summary>
        /// <param name="caller">The calling object or null.</param>
        /// <param name="message">The object to log.</param>
        /// <param name="replacements">String replacements.</param>
        public static void Info( object message, params object[] replacements)
        {
            Out(
                LogLevel.Info,
                message,
                replacements);
        }

        /// <summary>
        /// Logs a warning level message.
        /// </summary>
        /// <param name="caller">The calling object or null.</param>
        /// <param name="message">The object to log.</param>
        /// <param name="replacements">String replacements.</param>
        public static void Warning(object message, params object[] replacements)
        {
            Out(
                LogLevel.Warning,
                message,
                replacements);
        }

        /// <summary>
        /// Logs an error level message.
        /// </summary>
        /// <param name="caller">The calling object or null.</param>
        /// <param name="message">The object to log.</param>
        /// <param name="replacements">String replacements.</param>
        public static void Error(object message, params object[] replacements)
        {
            Out(
                LogLevel.Error,
                message,
                replacements);
        }

        /// <summary>
        /// Logs a message at a specific level.
        /// </summary>
        private static void Out(LogLevel level, object message, params object[] replacements)
        {
            if (_target == null)
            {
                return;
            }

            if (replacements.Length > 0)
            {
                message = string.Format(message.ToString(), replacements);
            }
            _target.OnLog(level, message.ToString());
        }
    }
}