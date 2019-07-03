using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Misaka.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Misaka.Config
{
    public class Configuration
    {
        public static Configuration Instance => new Configuration();

        public IConfiguration ConfigurationCore { get; private set; } 

        private Configuration()
        {
            ConfigurationCore = new ConfigurationBuilder().Build();
        }

        public IConfigurationSection GetSection(string key)
        {
            return Instance.ConfigurationCore?.GetSection(key);
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return Instance.ConfigurationCore?.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return Instance.ConfigurationCore?.GetReloadToken();
        }

        public string this[string key]
        {
            get => Instance.ConfigurationCore?[key];
            set => Instance.ConfigurationCore[key] = value;
        }


        public Configuration UseConfiguration(IConfiguration configuration)
        {
            ConfigurationCore = configuration ?? throw new ArgumentNullException(nameof(configuration));
            ObjectProviderFactory.Instance.RegisterInstance(typeof(Configuration), this);

            return this;
        }
    }
}
