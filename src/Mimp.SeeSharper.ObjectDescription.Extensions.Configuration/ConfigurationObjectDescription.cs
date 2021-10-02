using Microsoft.Extensions.Configuration;
using Mimp.SeeSharper.ObjectDescription.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimp.SeeSharper.ObjectDescription.Extensions.Configuration
{
    public class ConfigurationObjectDescription : IObjectDescription
    {


        public IConfiguration Configuration { get; }


        public bool HasValue => Configuration is IConfigurationSection s && s.Value is not null;

        public object? Value => (Configuration as IConfigurationSection)?.Value;

        public IEnumerable<KeyValuePair<string?, IObjectDescription>>? Children => HasValue ? null
            : Configuration.GetChildren().Select(c => new KeyValuePair<string?, IObjectDescription>(c.Key, new ConfigurationObjectDescription(c)));


        public ConfigurationObjectDescription(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


    }
}
