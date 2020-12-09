using DbUp.Engine;
using DbUp.Engine.Output;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Gofer.Core.Migrations
{
    public class UpgradeLogger : IUpgradeLog
    {
        private readonly ILogger<UpgradeEngine> _logger;

        public UpgradeLogger(ILogger<UpgradeEngine> logger)
        {
            _logger = logger ?? NullLogger<UpgradeEngine>.Instance;
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logger.Log(LogLevel.Information, format, args);
        }

        public void WriteError(string format, params object[] args)
        {
            _logger.Log(LogLevel.Error, format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.Log(LogLevel.Warning, format, args);
        }
    }
}