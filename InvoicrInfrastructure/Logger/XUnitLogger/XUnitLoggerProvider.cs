using Microsoft.Extensions.Logging;
using Xunit.Abstractions;


namespace InvoicrInfrastructure.Logger.XUnitLogger
{
    // Custom logger provider to redirect output to ITestOutputHelper
    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _outputHelper;

        public XUnitLoggerProvider(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(_outputHelper);
        }

        public void Dispose()
        {
            // Dispose logic if needed
        }
    }
}
