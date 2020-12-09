using Microsoft.Extensions.Logging;

namespace Gofer.Core.DependencyInjection
{
    public class GoferOptions
    {
        public string ConnectionString { get; set; }
        public bool GenerateReport { get; set; }
        public string ReportOutputPath { get; set; }
        public LogLevel Verbosity { get; set; } = LogLevel.Warning;
    }
}