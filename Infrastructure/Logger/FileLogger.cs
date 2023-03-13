using Microsoft.Extensions.Logging;

namespace Ifrastructure.Logger
{
    public class FileLogger : ILogger, IDisposable
    {
        readonly string filePath;
        static readonly object _lock = new();
        public FileLogger(string path)
        {
            filePath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose() { }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId,
                    TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_lock)
            {
                if (logLevel > LogLevel.Warning || logLevel > LogLevel.Error || logLevel > LogLevel.Critical)
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine + DateTime.Now + '\t');
            }
        }
    }
}
