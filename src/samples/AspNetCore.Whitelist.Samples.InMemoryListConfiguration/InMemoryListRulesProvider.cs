using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.Whitelist.Samples.InMemoryListConfiguration
{
    public class InMemoryListRulesProvider : IIpRulesProvider
    {
        public Task<IpRule[]> GetIpRules()
        {
            return Task.FromResult(new List<IpRule>()
            {
                // blacklist
                new IpRule(IpAddressRangeWithWildcard.Parse("127.0.0.4"),
                    IpRuleType.Blacklist),
                new IpRule(IpAddressRangeWithWildcard.Parse("127.0.0.4"),
                    IpRuleType.Blacklist),
                
                // whitelist
                new IpRule(IpAddressRangeWithWildcard.GetWildcardRange(),IpRuleType.Whitelist)
            }.ToArray());
        }
    }
}
