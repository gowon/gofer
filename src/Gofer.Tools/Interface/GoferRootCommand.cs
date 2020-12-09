using System.CommandLine;
using System.CommandLine.Invocation;

namespace Gofer.Tools.Interface
{
    public class GoferRootCommand : RootCommand
    {
        public GoferRootCommand()
        {
            Initialize();
        }

        protected void Initialize()
        {
            Name = GetType().Assembly.GetName().Name!.ToLowerInvariant();

            AddCommand(new CreateCommand());
            AddCommand(new MigrateCommand());

            Handler = CommandHandler.Create<IConsole>(console => { new GoferHelpBuilder(console).Write(this); });
        }
    }
}