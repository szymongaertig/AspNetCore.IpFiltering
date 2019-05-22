using System.Net;
using System.Threading.Tasks;

namespace AspNetCore.IpFiltering
{
    public interface IIpAddressValidator
    {
        Task<bool> AllowAccess(IPAddress ipAddress);
    }
}