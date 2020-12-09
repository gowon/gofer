using System;
using System.Data.Common;
using System.Data.SqlClient;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Core;

namespace Gofer.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGoferTools(this IServiceCollection services,
            LoggingLevelSwitch loggingLevelSwitch)
        {
            services.Configure<HostOptions>(options => options.ShutdownTimeout = TimeSpan.FromSeconds(10));
            services.ConfigureOptions<ConfigureGoferOptions>();
            services.AddSingleton(loggingLevelSwitch);
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
            services.AddSingleton<Func<string, DbConnection>>(provider => { return s => new SqlConnection(s); });

            return services;
        }
    }
}