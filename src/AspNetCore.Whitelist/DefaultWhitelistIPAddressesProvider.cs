using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTools;

namespace AspNetCore.Whitelist
{
    public class DefaultWhitelistIpAddressesProvider : IWhitelistIpAddressesProvider
    {
        private ICollection<IpAddressRangeWithWildcard> _whitelist;
        private ICollection<IpAddressRangeWithWildcard> _blacklist;

        public DefaultWhitelistIpAddressesProvider(
            string[] whitelist,
            string[] blacklist)
        {
            if (whitelist == null)
            {
                _whitelist = new List<IpAddressRangeWithWildcard>();
            }
            else
            {
                _whitelist = ConvertToRange(whitelist);
            }

            if (blacklist == null)
            {
                _blacklist = new List<IpAddressRangeWithWildcard>();
            }
            else
            {
                _blacklist = ConvertToRange(blacklist);
            }            
        }

        private ICollection<IpAddressRangeWithWildcard> ConvertToRange(string[] ipAddressesList)
        {
            return ipAddressesList
                .Distinct()
                .Select(x => x.ConvertToIpRange())
                .ToArray();
        }

        public Task<IpAddressRangeWithWildcard[]> GetWhitelist()
        {
            return Task.FromResult(_whitelist.ToArray());
        }

        public Task<IpAddressRangeWithWildcard[]> GetBlacklist()
        {
            return Task.FromResult(_blacklist.ToArray());
        }
    }
}