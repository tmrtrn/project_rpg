namespace Core.Services.Logging
{
    public interface ILogTarget
    {
        /// <summary>
        /// Only logs with a level greater to or equal to this level will be respected.
        /// </summary>
        LogLevel Filter { get; set; }
        /// <summary>
        /// If true, prepends the log level to the log. This defaults to true.
        /// </summary>
        public bool Level { get; set; }
        void OnLog(LogLevel level, string message);
    }
}