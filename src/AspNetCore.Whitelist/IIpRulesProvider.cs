using System.Threading.Tasks;
using NetTools;

namespace AspNetCore.Whitelist
{
    public interface IIpRulesProvider
    {
        Task<IpRule[]> GetIpRules();
    }
}