using Microsoft.Extensions.Logging;

namespace Ifrastructure.Logger
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
