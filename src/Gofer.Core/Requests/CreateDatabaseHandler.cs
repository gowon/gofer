using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Gofer.Core.Migrations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gofer.Core.Requests
{
    public class CreateDatabaseHandler : AsyncRequestHandler<CreateDatabase>
    {
        private readonly Func<string, DbConnection> _connectionFactory;
        private readonly ILogger<UpgradeEngine> _logger;

        public CreateDatabaseHandler(Func<string, DbConnection> connectionFactory,
            ILogger<UpgradeEngine> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        protected override Task Handle(CreateDatabase request, CancellationToken cancellationToken)
        {
            var script = request.ForceDeleteAndCreate
                ? (IScript) new SqlServerDeleteAndCreateDatabaseScript(request.DatabaseName)
                : new SqlServerCreateDatabaseScript(request.DatabaseName);

            var upgradeEngine = DeployChanges.To
                .SqlDatabase(
                    new DbConnectionFactoryManager(() => _connectionFactory(request.ConnectionString)))
                .WithScript($"Create Database '{request.DatabaseName}'", script)
                .LogTo(_logger)
                .JournalTo(new NullJournal())
                .Build();

            var stopwatch = Stopwatch.StartNew();
            var result = upgradeEngine.PerformUpgrade();
            stopwatch.Stop();

            _logger.LogInformation("Operation completed in {DurationMilliseconds}ms", stopwatch.ElapsedMilliseconds);

            if (!result.Successful) throw result.Error;
            return Task.CompletedTask;
        }
    }
}