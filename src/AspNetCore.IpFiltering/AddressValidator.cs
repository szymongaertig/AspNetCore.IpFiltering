using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCore.IpFiltering
{
    public class AddressValidator : IIpAddressValidator
    {
        private readonly IIpRulesProvider _rulesProvider;
        private ILogger<AddressValidator> _logger;

        public AddressValidator(IIpRulesProvider rulesProvider,
            ILogger<AddressValidator> logger)
        {
            _rulesProvider = rulesProvider;
            _logger = logger;
        }

        public async Task<bool> AllowAccess(IPAddress ipAddress)
        {
            bool existsOnWhitelist = false;
            bool existsOnBlacklist = false;

            var ipRules = await _rulesProvider.GetIpRules();
            var blackList = ipRules.Where(x => x.Type == IpRuleType.Blacklist).Select(x => x.AddressRange);
            existsOnBlacklist = blackList.Any(x => x.Contains(ipAddress));
            
            var whiteList = ipRules.Where(x => x.Type == IpRuleType.Whitelist).Select(x => x.AddressRange);
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