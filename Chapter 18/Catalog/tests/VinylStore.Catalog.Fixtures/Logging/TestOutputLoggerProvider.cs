using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace VinylStore.Catalog.Fixtures.Logging
{
    public class TestOutputLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, TestOutputLogger> _loggers =
            new ConcurrentDictionary<string, TestOutputLogger>();

        private readonly ITestOutputHelper _testOutputHelper;

        public TestOutputLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new TestOutputLogger(_testOutputHelper));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
