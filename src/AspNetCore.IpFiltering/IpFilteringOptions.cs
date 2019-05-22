namespace AspNetCore.IpFiltering
{
    public class IpFilteringOptions
    {
        public string[] Blacklist { get; set; }
        public string[] Whitelist { get; set; }
        public IpRulesSource IpRulesSource { get; set; } = IpRulesSource.Configuration;
        public IpRuleCacheSource IpRuleCacheSource { get; set; } = IpRuleCacheSource.Configuration;
        public int? DefaultIpRuleCacheDuration { get; set; } = 60;
        public int FailureHttpStatusCode { get; set; } = 404;
    }
}