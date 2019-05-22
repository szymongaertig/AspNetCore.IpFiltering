using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.IpFiltering
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIpFiltering(this IServiceCollection collection,
            IpFilteringOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            ConfigureIpFiltering(collection, options);
        }

        private static void ConfigureIpFiltering(IServiceCollection serviceCollection,
            IpFilteringOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            serviceCollection.AddSingleton<IpFilteringOptions>(options);

            if (options.IpRulesSource == IpRulesSource.Configuration)
            {
                serviceCollection.AddSingleton<IIpRulesProvider>(new DefaultIpRulesProvider(
                    options.Whitelist,
                    options.Blacklist));
            }

            if (options.IpRulesCacheSource == IpRulesCacheSource.Configuration)
            {
                serviceCollection.AddSingleton<IIpAddressResultCache>(new DefaultIpResultCache(options.IpRulesSource == IpRulesSource.Configuration
                                                                                               ? options.DefaultIpRuleCacheDuration
                                                                                               : null));
            }
            
            serviceCollection.AddSingleton<IIpAddressValidator, AddressValidator>();
        }

        public static void AddIpFiltering(this IServiceCollection collection,
            IConfigurationSection configurationSection)
        {
            if (configurationSection == null)
            {
                throw new ConfigurationErrorsException("Could not find provided configuration section");
            }

            var options = configurationSection.Get<IpFilteringOptions>();
            ConfigureIpFiltering(collection, options);
        }
    }
}