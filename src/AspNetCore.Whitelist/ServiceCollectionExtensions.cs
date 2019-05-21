using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Whitelist
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWhiteList(this IServiceCollection collection,
            WhitelistOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            ConfigureWhitelist(collection, options);
        }

        private static void ConfigureWhitelist(IServiceCollection serviceCollection,
            WhitelistOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            serviceCollection.AddSingleton<WhitelistOptions>(options);

            if (options.IpListSource == IpListSource.Configuration)
            {
                serviceCollection.AddSingleton<IWhitelistIpAddressesProvider>(new DefaultWhitelistIpAddressesProvider(
                    options.Whitelist,
                    options.Blacklist));
            }

            serviceCollection.AddSingleton<IIpAddressValidator, AddressValidator>();
        }

        public static void AddWhiteList(this IServiceCollection collection,
            IConfigurationSection configurationSection)
        {
            if (configurationSection == null)
            {
                throw new ConfigurationErrorsException("Could not find provided configuration section");
            }

            var options = configurationSection.Get<WhitelistOptions>();
            ConfigureWhitelist(collection, options);
        }
    }
}