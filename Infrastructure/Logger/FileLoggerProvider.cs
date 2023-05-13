using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        readonly string path;
        public FileLoggerProvider(string path)
        {
            this.path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose() { }
    }
}
