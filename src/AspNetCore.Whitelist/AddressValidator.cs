using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Whitelist
{
    public class AddressValidator : IIpAddressValidator
    {
        private readonly IWhitelistIpAddressesProvider _addressesProvider;
        private ILogger<AddressValidator> _logger;

        public AddressValidator(IWhitelistIpAddressesProvider addressesProvider,
            ILogger<AddressValidator> logger)
        {
            _addressesProvider = addressesProvider;
            _logger = logger;
        }

        public async Task<bool> AllowAccess(IPAddress ipAddress)
        {
            bool existsOnWhitelist = false;
            bool existsOnBlacklist = false;

            var blackList = await _addressesProvider.GetBlacklist();
            existsOnBlacklist = blackList.Any(x => x.Contains(ipAddress));

            var whiteList = await _addressesProvider.GetWhitelist();
            existsOnWhitelist = whiteList.Any(x => x.Contains(ipAddress));

            if (existsOnBlacklist)
            {
                if (existsOnWhitelist)
                {
                    _logger.LogWarning("IP {IP} exists on both Whitelist and Blacklist", ipAddress);
                }

                return false;
            }

            if (existsOnWhitelist)
            {
                return true;
            }

            return false;
        }
    }
}