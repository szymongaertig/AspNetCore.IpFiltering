namespace AspNetCore.IpFiltering
{
    public class IpRule
    {
        public IpRule()
        {
            
        }
        public IpRule(IpAddressRangeWithWildcard addressRange, IpRuleType type)
        {
            AddressRange = addressRange;
            Type = type;
        }

        public IpAddressRangeWithWildcard AddressRange { get; set; }
        public IpRuleType Type { get; set; }
    }
}