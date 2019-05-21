namespace AspNetCore.Whitelist
{
    public static class StringExtensions
    {
        public static IpAddressRangeWithWildcard ConvertToIpRange(this string ipAddressesStr)
        {
            if (ipAddressesStr.Equals("*"))
            {
                return IpAddressRangeWithWildcard.GetWildcardRange();
            }
            else
            {
                return IpAddressRangeWithWildcard.Parse(ipAddressesStr);
            }
        }
    }
}