using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Gofer.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Serilog.Events;

namespace Gofer.Tools.Interface
{
    public static class CommandLineBuilderExtensions
    {
        public static readonly LogLevel DefaultLogLevel = LogLevel.Warning;


        public static T GetService<T>(this InvocationContext context)
        {
            // https://github.com/dotnet/command-line-api/issues/1025#issuecomment-729271227
            // https://github.com/dotnet/command-line-api/issues/974#issuecomment-679167822
            return (T) new ModelBinder(typeof(T)).CreateInstance(context.BindingContext);
        }

        public static CommandLineBuilder UseVerbositySwitch(
            this CommandLineBuilder builder, LoggingLevelSwitch loggingLevelSwitch)
        {
            loggingLevelSwitch = loggingLevelSwitch ?? throw new ArgumentNullException(nameof(loggingLevelSwitch));

            if (builder.Command.Children.GetByAlias("--verbosity") != null) return builder;

            var versionOption = new Option<LogLevel>(new[] {"-v", "/v", "--verbosity"},
                "Set output verbosity");

            builder.AddGlobalOption(versionOption);

            builder.UseMiddleware(async (context, next) =>
            {
                var result = context.ParseResult.FindResultFor(versionOption);
                var host = context.GetService<IHost>();
                var options = host.Services.GetRequiredService<IOptions<GoferOptions>>();
                var verbosity = result?.GetValueOrDefault<LogLevel>() ?? options.Value.Verbosity;

                LogEventLevel minimumLevel;
                switch (verbosity)
                {
                    case LogLevel.Trace:
                        minimumLevel = LogEventLevel.Verbose;
                        break;
                    case LogLevel.Debug:
                        minimumLevel = LogEventLevel.Debug;
                        break;
                    case LogLevel.Information:
                        minimumLevel = LogEventLevel.Information;
                        break;
                    case LogLevel.Warning:
                        minimumLevel = LogEventLevel.Warning;
                        break;
                    case LogLevel.Error:
                        minimumLevel = LogEventLevel.Error;
                        break;
                    case LogLevel.Critical:
                    case LogLevel.None:
                        minimumLevel = LogEventLevel.Fatal;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(verbosity));
                }

                loggingLevelSwitch.MinimumLevel = minimumLevel;
                await next(context);
            });

            return builder;
        }
    }
}