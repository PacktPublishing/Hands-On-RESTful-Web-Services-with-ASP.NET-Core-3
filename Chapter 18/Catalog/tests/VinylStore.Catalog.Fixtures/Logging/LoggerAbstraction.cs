using System;
using Microsoft.Extensions.Logging;

namespace VinylStore.Catalog.Fixtures.Logging
{
    public abstract class LoggerAbstraction<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
            => throw new NotImplementedException();

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
            => Log(logLevel, exception, formatter(state, exception));

        public abstract void Log(LogLevel logLevel, Exception ex, string information);
    }
}
