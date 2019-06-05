namespace AspNetCore.IpFiltering
{
    public interface IIpFilteringOptions
    {
        string[] Blacklist { get; set; }
        string[] Whitelist { get; set; }
        IpRulesSource IpRulesSource { get; set; }
        IpRulesCacheSource IpRulesCacheSource { get; set; }
        int? DefaultIpRuleCacheDuration { get; set; }
        int FailureHttpStatusCode { get; set; }
        string FailureMessage { get; set; }
        string[] IgnoredPaths { get; set; }
        bool LearningMode { get; set; }
    }

    public class IpFilteringOptions : IIpFilteringOptions
    {
        public string[] Blacklist { get; set; }
        public string[] Whitelist { get; set; }
        public IpRulesSource IpRulesSource { get; set; } = IpRulesSource.Configuration;
        public IpRulesCacheSource IpRulesCacheSource { get; set; } = IpRulesCacheSource.Configuration;
        public int? DefaultIpRuleCacheDuration { get; set; } = 60;
        public int FailureHttpStatusCode { get; set; } = 404;
        public string FailureMessage { get; set; } = "You shall not pass";
        public string[] IgnoredPaths { get; set; }
        public bool LearningMode { get; set; } = false;
    }
}