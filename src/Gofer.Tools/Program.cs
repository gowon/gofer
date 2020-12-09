using System;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Gofer.Core.DependencyInjection;
using Gofer.Tools.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Gofer.Tools
{
    internal class Program
    {
        public static LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch(LogEventLevel.Error);

        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .WriteTo.Debug()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                // https://github.com/couven92/dotnet-command-line-api/blob/hosting-sample/samples/HostedConsole/Program.cs
                var parser = new CommandLineBuilder(new GoferRootCommand())
                    .UseHost(CreateHostBuilder)
                    .UseVerbositySwitch(LevelSwitch)
                    .UseDefaults()
                    .UseHelpBuilder(context => new GoferHelpBuilder(context.Console))
                    .UseExceptionHandler((exception, context) =>
                    {
                        Log.Fatal(exception, "Unhandled exception occurred.");
                    })
                    .Build();

                return await parser.InvokeAsync(args);
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Application terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                    AddConfiguration(config, hostingContext.HostingEnvironment, args))
                .ConfigureLogging((context, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddSerilog();
                })
                .ConfigureServices((hostContext, services) => { services.AddGoferTools(LevelSwitch); });
        }

        private static IConfigurationBuilder AddConfiguration(
            IConfigurationBuilder configurationBuilder = null,
            IHostEnvironment hostingEnvironment = null,
            params string[] args)
        {
            // DEVNOTE ConfigurationBuilder is additive. Settings can be added and replaced with this process, but
            // not removed. If you require a different shape for a particular configuration, it is best to
            // remove a "default" from the base configuration, and specify in every environment, including Local
            // https://www.paraesthesia.com/archive/2018/06/20/microsoft-extensions-configuration-deep-dive/

            configurationBuilder ??= new ConfigurationBuilder();
            return configurationBuilder
                // Add configuration from the appsettings.json file.
                .AddJsonFile("gofer.json", true, false)
                // Add configuration specific to the Development, Staging or Production environments. This config can
                // be stored on the machine being deployed to or if you are using Azure, in the cloud. These settings
                // override the ones in all of the above config files. See
                // http://docs.asp.net/en/latest/security/app-secrets.html
                .AddEnvironmentVariables()
                // Add command line options. These take the highest priority.
                .AddCommandLine(args);
        }
    }
}