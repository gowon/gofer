using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Gofer.Core.DependencyInjection
{
    public class ConfigureGoferOptions : IConfigureOptions<GoferOptions>
    {
        private readonly IConfiguration _configuration;

        public ConfigureGoferOptions(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Configure(GoferOptions options)
        {
            _configuration.Bind(options);
        }
    }
}