namespace Ocelot.UnitTests.Logging
{
    using Moq;
    using Xunit;
    using Ocelot.Logging;
    using Microsoft.Extensions.Logging;
    using Ocelot.Infrastructure.RequestData;
    using System;

    public class AspDotNetLoggerTests
    {
        private readonly Mock<ILogger<object>> _coreLogger;
        private readonly AspDotNetLogger _logger; 
        private Mock<IRequestScopedDataRepository> _repo;
        private readonly string _b;
        private readonly string _a;
        private readonly Exception _ex;

        public AspDotNetLoggerTests()
        {
            _a = "tom";
            _b = "laura";
            _ex = new Exception("oh no");
            _coreLogger = new Mock<ILogger<object>>();
            _repo = new Mock<IRequestScopedDataRepository>();
            _logger = new AspDotNetLogger(_coreLogger.Object, _repo.Object);
        }

        [Fact]
        public void should_log_trace()
        {
            _logger.LogTrace($"a message from {_a} to {_b}");

            ThenLevelIsLogged("requestId: no request id, previousRequestId: no previous request id, message: a message from tom to laura", LogLevel.Trace);
        }

        [Fact]
        public void should_log_info()
        {
            _logger.LogInformation($"a message from {_a} to {_b}");

            ThenLevelIsLogged("requestId: no request id, previousRequestId: no previous request id, message: a message from tom to laura", LogLevel.Information);
        }

        [Fact]
        public void should_log_warning()
        {
            _logger.LogWarning($"a message from {_a} to {_b}");

            ThenLevelIsLogged("requestId: no request id, previousRequestId: no previous request id, message: a message from tom to laura", LogLevel.Warning);
        }

        [Fact]
        public void should_log_error()
        {
            _logger.LogError($"a message from {_a} to {_b}", _ex);

            ThenLevelIsLogged("requestId: no request id, previousRequestId: no previous request id, message: a message from tom to laura, exception: System.Exception: oh no", LogLevel.Error);
        }

        [Fact]
        public void should_log_critical()
        {
            _logger.LogCritical($"a message from {_a} to {_b}", _ex);

            ThenLevelIsLogged("requestId: no request id, previousRequestId: no previous request id, message: a message from tom to laura, exception: System.Exception: oh no", LogLevel.Critical);
        }

        private void ThenLevelIsLogged(string expected, LogLevel expectedLogLevel)
        {
            _coreLogger.Verify(
                x => x.Log(
                    expectedLogLevel,
                    It.IsAny<EventId>(),
                    It.Is<object>(o => o.ToString() == expected), 
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }
    }
}
