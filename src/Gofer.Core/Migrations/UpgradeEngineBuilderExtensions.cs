using System.Reflection;
using DbUp.Builder;
using DbUp.Engine;
using Microsoft.Extensions.Logging;

namespace Gofer.Core.Migrations
{
    public static class UpgradeEngineBuilderExtensions
    {
        public static readonly string AssemblyNamespace =
            nameof(UpgradeEngineBuilderExtensions);

        public static UpgradeEngineBuilder WithMigrations(this UpgradeEngineBuilder builder, string databaseName,
            ILogger<UpgradeEngine> logger = null)
        {


            return builder
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    script => script.StartsWith($"{AssemblyNamespace}.Scripts.{databaseName}.Migrations."))
                .LogTo(logger)
                .WithTransactionPerScript()
                .JournalToSqlTable("dbo", "_MigrationsJournal");
        }

        public static UpgradeEngineBuilder WithMigrations(this UpgradeEngineBuilder builder,
            ILogger<UpgradeEngine> logger = null)
        {


            return builder
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    script => script.StartsWith($"{AssemblyNamespace}.Scripts.Migrations."))
                .LogTo(logger)
                .WithTransactionPerScript()
                .JournalToSqlTable("dbo", "_MigrationsJournal");
        }

        public static UpgradeEngineBuilder WithPreDeployments(this UpgradeEngineBuilder builder,
            ILogger<UpgradeEngine> logger)
        {
            return builder
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    script => script.StartsWith($"{AssemblyNamespace}.Scripts.PreDeployment."))
                .LogTo(logger)
                .WithTransactionPerScript()
                .JournalToSqlTable("dbo", "_PreDeploymentJournal");
        }

        public static UpgradeEngineBuilder WithPostDeployments(this UpgradeEngineBuilder builder,
            ILogger<UpgradeEngine> logger)
        {
            return builder
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    script => script.StartsWith($"{AssemblyNamespace}.Scripts.PostDeployment."))
                .LogTo(logger)
                .WithTransactionPerScript()
                .JournalToSqlTable("dbo", "_PostDeploymentJournal");
        }

        public static UpgradeEngineBuilder LogTo(this UpgradeEngineBuilder builder,
            ILogger<UpgradeEngine> logger)
        {
            return builder.LogTo(new UpgradeLogger(logger));
        }
    }
}