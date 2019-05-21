using System.Threading.Tasks;

namespace AspNetCore.Whitelist.Samples.InMemoryListConfiguration
{
    public class InMemoryListAddressesProvider : IWhitelistIpAddressesProvider
    {
        public Task<IpAddressRangeWithWildcard[]> GetBlacklist()
        {
            return Task.FromResult(new[] {
                IpAddressRangeWithWildcard.Parse("127.0.0.4"),
                IpAddressRangeWithWildcard.Parse("127.0.0.5")
            });
        }

        public Task<IpAddressRangeWithWildcard[]> GetWhitelist()
        {
            return Task.FromResult(new[] {
                IpAddressRangeWithWildcard.GetWildcardRange()
            });
        }
    }
}
