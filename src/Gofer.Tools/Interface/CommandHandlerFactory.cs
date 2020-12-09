using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Gofer.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gofer.Tools.Interface
{
    public static class CommandHandlerFactory
    {
        public static ICommandHandler CreateFor<TRequest>() where TRequest : IBaseRequest, IParseResultMapper, new()
        {
            return CommandHandler.Create(async (IHost host, ParseResult parseResult) =>
            {
                var mediator = host.Services.GetRequiredService<IMediator>();
                var request = new TRequest();
                request.MapUsing(parseResult);
                await mediator.Send(request);
            });
        }
    }
}