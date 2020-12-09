using System.CommandLine;
using System.IO;
using Gofer.Core.Requests;

namespace Gofer.Tools.Interface
{
    public class MigrateCommand : Command
    {
        public MigrateCommand() : base("migrate", "Perform database migrations")
        {
            Initialize();
        }

        protected void Initialize()
        {
            // arguments
            AddArgument(new Argument<DirectoryInfo>("migrations-directory",
                    "Path to migration scripts (leave blank for current directory)")
                {Arity = ArgumentArity.ZeroOrOne});

            // options
            AddOption(new Option<string>(new[] {"-c", "/c", "--connection-string"},
                "Specify SQL Server connection string"));

            AddOption(new Option<bool>(new[] {"-d", "/d", "--dryrun"},
                "Perform a dry-run instead of executing migration scripts"));

            AddOption(new Option<string>(new[] {"-j", "/j", "--journal"},
                () => "dbo._MigrationJournal",
                "Specify journal schema and name"));

            AddOption(new Option<int>(new[] {"-l", "/l", "--limit"},
                "Specify a limit on the amount of scripts to execute from the current position in the migration journal. A negative number will run all but N"));

            AddOption(new Option<FileInfo>(new[] {"-o", "/o", "--output"},
                "Specify custom path for the HTML report"));

            AddOption(new Option<bool>(new[] {"-p", "/p", "--pretend"},
                "Journal scripts without actually executing content"));

            AddOption(new Option<bool>(new[] {"-r", "/r", "--report"},
                "Generate an HTML report of the scripts to be executed during the migration"));

            // handler
            Handler = CommandHandlerFactory.CreateFor<PerformMigration>();
        }
    }
}