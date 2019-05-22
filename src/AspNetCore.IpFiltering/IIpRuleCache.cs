using System.Net;
using System.Threading.Tasks;

namespace AspNetCore.IpFiltering
{
    public interface IIpAddressResultCache
    {
        Task<bool?> AllowAddress(IPAddress remoteAddress);
        Task SaveResult(IPAddress remoteAddress, bool result);
    }
}