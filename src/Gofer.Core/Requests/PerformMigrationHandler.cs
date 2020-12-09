using System;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Gofer.Core.Migrations;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gofer.Core.Requests
{
    public class PerformMigrationHandler : AsyncRequestHandler<PerformMigration>
    {
        private readonly Func<string, DbConnection> _connectionFactory;
        private readonly IHostEnvironment _environment;
        private readonly ILogger<UpgradeEngine> _logger;

        public PerformMigrationHandler(Func<string, DbConnection> connectionFactory,
            ILogger<UpgradeEngine> logger, IHostEnvironment environment)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _environment = environment;
        }

        protected override Task Handle(PerformMigration request, CancellationToken cancellationToken)
        {
            var directory = request.MigrationsDirectory?.FullName ?? _environment.ContentRootPath;

            // determine journal table name
            var journalNamespace = request.JournalTableName.Split('.');
            var schema = journalNamespace.Length == 2 ? journalNamespace[0] : "dbo";
            var table = journalNamespace.Length == 2 ? journalNamespace[1] : journalNamespace[0];

            // scaffold engine
            var builder = DeployChanges.To
                .SqlDatabase(
                    new DbConnectionFactoryManager(() => _connectionFactory(request.ConnectionString)))
                .WithFilter(new ExecutionLimitScriptFilter(request.ExecutionLimit))
                .WithScriptsFromFileSystem(directory)
                .LogTo(_logger)
                .WithTransactionPerScript()
                .JournalToSqlTable(schema, table);

            if (request.HasReport)
            {
                var path = request.ReportPath?.FullName ??
                           Path.GetFullPath($"{DateTime.Now:yyyyMMddHHmmss}_migrations.html",
                               _environment.ContentRootPath);

                _logger.LogInformation("Generating HTML report of scripts to execute");
                builder.Build().GenerateUpgradeHtmlReport(path);
                _logger.LogInformation("Created HTML report at '{Path}'", path);
            }

            if (request.IsDryRun)
            {
                _logger.LogInformation("Dry Run execution");
                var scripts = builder.Build().GetScriptsToExecute();

                foreach (var script in scripts)
                    _logger.LogInformation("[DRY] Executing Database Server script '{ScriptName}'",
                        script.Name);

                return Task.CompletedTask;
            }

            if (request.HasPretendExecution)
            {
                _logger.LogInformation("Pretend execution");
                builder = builder.WithPreprocessor(new PretendExecutionPreProcessor());
            }

            var stopwatch = Stopwatch.StartNew();
            var result = builder.Build().PerformUpgrade();
            stopwatch.Stop();

            _logger.LogInformation("Operation completed in {DurationMilliseconds}ms", stopwatch.ElapsedMilliseconds);

            if (!result.Successful) throw result.Error;
            return Task.CompletedTask;
        }
    }
}