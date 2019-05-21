using System.Threading.Tasks;
using NetTools;

namespace AspNetCore.Whitelist
{
    public interface IWhitelistIpAddressesProvider
    {
        Task<IpAddressRangeWithWildcard[]> GetWhitelist();
        Task<IpAddressRangeWithWildcard[]> GetBlacklist();
    }
}