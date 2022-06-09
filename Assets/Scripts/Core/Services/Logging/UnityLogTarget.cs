using System.Text;
using UnityEngine;

namespace Core.Services.Logging
{
    /// <summary>
    /// Targets Unity logging methods.
    /// </summary>
    public class UnityLogTarget : ILogTarget
    {
        public LogLevel Filter { get; set; }

        public bool Level { get; set; } = true;

        public void OnLog(LogLevel level, string message)
        {
            if (level < Filter)
            {
                return;
            }

            var formattedMessage = Format(level, message);

            switch (level)
            {
                case LogLevel.Info:
                case LogLevel.Debug:
                {
                    Debug.Log(formattedMessage);
                    break;
                }
                case LogLevel.Warning:
                {
                    Debug.LogWarning(formattedMessage);
                    break;
                }
                case LogLevel.Error:
                {
                    Debug.LogError(formattedMessage);
                    break;
                }
            }
        }

        private string Format(LogLevel level, string message)
        {
            var log = new StringBuilder();
            if (Level)
            {
                log.AppendFormat("[{0}]", level);
            }
            log.AppendFormat("{0}\n", message);
            return log.ToString();
        }
    }
}