namespace AspNetCore.Whitelist
{
    public class WhitelistOptions
    {
        public string[] Blacklist { get; set; }
        public string[] Whitelist { get; set; }
        public IpListSource IpListSource { get; set; }
        public int FailureHttpStatusCode { get; set; } = 404;
    }
}