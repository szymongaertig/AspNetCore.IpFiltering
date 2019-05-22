using System.Net;
using System.Threading.Tasks;

namespace AspNetCore.Whitelist
{
    public interface IIpAddressResultCache
    {
        Task<bool?> AllowAddress(IPAddress remoteAddress);
        Task SaveResult(IPAddress remoteAddress, bool result);
    }
}