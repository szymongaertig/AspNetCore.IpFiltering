using System.Net;
using NetTools;

namespace AspNetCore.IpFiltering
{
    public class IpAddressRangeWithWildcard
    {
        private IPAddressRange _base;
        private bool _wildCard = false;

        public IpAddressRangeWithWildcard(IPAddressRange addressRange)
        {
            _base = addressRange;
            _wildCard = false;   
        }
        
        private IpAddressRangeWithWildcard(bool wildCard)
        {
            _wildCard = wildCard;
        }
        
        public static IpAddressRangeWithWildcard GetWildcardRange()
        {
            return new IpAddressRangeWithWildcard(true);
        }
        public static IpAddressRangeWithWildcard Parse(string ipAddress)
        {
            return new IpAddressRangeWithWildcard(IPAddressRange.Parse(ipAddress));
        }

        public bool Contains(IPAddress ipaddress)
        {
            if (_wildCard)
            {
                return true;
            }

            return _base.Contains(ipaddress);
        }
    }
}