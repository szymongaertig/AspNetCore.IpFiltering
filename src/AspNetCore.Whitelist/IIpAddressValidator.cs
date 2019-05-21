using System.Net;
using System.Threading.Tasks;

namespace AspNetCore.Whitelist
{
    public interface IIpAddressValidator
    {
        Task<bool> AllowAccess(IPAddress ipAddress);
    }
}