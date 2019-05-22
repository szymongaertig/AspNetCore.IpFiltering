using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTools;

namespace AspNetCore.Whitelist
{
    public class DefaultIpRulesProvider : IIpRulesProvider
    {
        private List<IpRule> _rules;

        private void AddRules(string[] addresses, IpRuleType type)
        {
            _rules.AddRange(collection: addresses.Distinct()
                .Select(x => x.ConvertToIpRange())                
                .Select(x => new IpRule(x,type)));
        }
        
        public DefaultIpRulesProvider(
            string[] whitelist,
            string[] blacklist)
        {
            _rules = new List<IpRule>();
            
            if (whitelist != null)
            {
                AddRules(whitelist,IpRuleType.Whitelist);
            }

            if (blacklist != null)
            {
                AddRules(blacklist, IpRuleType.Blacklist);
            }            
        }

        public Task<IpRule[]> GetIpRules()
        {
            return Task.FromResult(_rules.ToArray());
        }
    }
}