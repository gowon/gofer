using System.CommandLine.Parsing;

namespace Gofer.Core
{
    public interface IParseResultMapper
    {
        void MapUsing(ParseResult parseResult);
    }
}