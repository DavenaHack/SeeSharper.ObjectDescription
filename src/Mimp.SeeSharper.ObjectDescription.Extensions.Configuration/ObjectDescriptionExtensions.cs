using Microsoft.Extensions.Configuration;
using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;

namespace Mimp.SeeSharper.ObjectDescription.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {


        public static IObjectDescription ToDescription(this IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            return new ConfigurationObjectDescription(configuration);
        }


    }
}
