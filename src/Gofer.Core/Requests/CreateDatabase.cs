using System.CommandLine.Parsing;
using MediatR;

namespace Gofer.Core.Requests
{
    public class CreateDatabase : IRequest, IParseResultMapper
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public bool ForceDeleteAndCreate { get; set; }

        public void MapUsing(ParseResult parseResult)
        {
            DatabaseName = parseResult.ValueForArgument<string>("database");
            ConnectionString = parseResult.ValueForOption<string>("--connection-string");
            ForceDeleteAndCreate = parseResult.ValueForOption<bool>("--force");
        }
    }
}