using System.Threading.Tasks;
using NetTools;

namespace AspNetCore.IpFiltering
{
    public interface IIpRulesProvider
    {
        Task<IpRule[]> GetIpRules();
    }
}