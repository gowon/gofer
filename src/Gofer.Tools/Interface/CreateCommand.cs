using System.CommandLine;
using Gofer.Core.Requests;

namespace Gofer.Tools.Interface
{
    public class CreateCommand : Command
    {
        public CreateCommand() : base("create", "Create a database")
        {
            Initialize();
        }

        protected void Initialize()
        {
            // arguments
            AddArgument(new Argument<string>("database",
                    "Specify the name of the database to be created")
                {Arity = ArgumentArity.ExactlyOne});

            // options
            AddOption(new Option<string>(new[] {"-c", "/c", "--connection-string"},
                "Specify SQL Server connection string"));

            AddOption(new Option<bool>(new[] {"-f", "/f", "--force"},
                "Force delete of database if it already exists"));

            // handler
            Handler = CommandHandlerFactory.CreateFor<CreateDatabase>();
        }
    }
}