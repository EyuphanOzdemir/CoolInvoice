using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrApp.Tests.Logger
{
    using Microsoft.Extensions.Logging;
    using Moq;

    public class MockLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            // Create a mock logger using Moq
            var mockLogger = new Mock<ILogger>();
            return mockLogger.Object;
        }

        public void Dispose()
        {
            // No-op for mock logger disposal
        }
    }
}
