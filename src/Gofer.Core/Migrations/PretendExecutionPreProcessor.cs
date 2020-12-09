using DbUp.Engine;

namespace Gofer.Core.Migrations
{
    public class PretendExecutionPreProcessor : IScriptPreprocessor
    {
        public string Process(string contents)
        {
            return string.Empty;
        }
    }
}