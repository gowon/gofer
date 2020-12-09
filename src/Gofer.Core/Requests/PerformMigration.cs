using System.CommandLine.Parsing;
using System.IO;
using MediatR;

namespace Gofer.Core.Requests
{
    public class PerformMigration : IRequest, IParseResultMapper
    {
        public string ConnectionString { get; set; }
        public DirectoryInfo MigrationsDirectory { get; set; }
        public int ExecutionLimit { get; set; }
        public bool IsDryRun { get; set; }
        public bool HasPretendExecution { get; set; }
        public bool HasReport { get; set; }
        public FileInfo ReportPath { get; set; }
        public string JournalTableName { get; set; }

        public void MapUsing(ParseResult parseResult)
        {
            MigrationsDirectory = parseResult.ValueForArgument<DirectoryInfo>("migrations-directory");
            IsDryRun = parseResult.ValueForOption<bool>("--dryrun");
            HasPretendExecution = parseResult.ValueForOption<bool>("--pretend");
            ExecutionLimit = parseResult.ValueForOption<int>("--limit");
            ConnectionString = parseResult.ValueForOption<string>("--connection-string");
            HasReport = parseResult.ValueForOption<bool>("--report");
            ReportPath = parseResult.ValueForOption<FileInfo>("--output");
            JournalTableName = parseResult.ValueForOption<string>("--journal");
        }
    }
}