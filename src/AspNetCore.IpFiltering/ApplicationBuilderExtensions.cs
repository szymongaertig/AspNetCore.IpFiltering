using AspNetCore.Whitelist;
using Microsoft.AspNetCore.Builder;

namespace AspNetCore.IpFiltering
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseIpFilteringMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<IpFilteringMiddleware>();
            return applicationBuilder;
        }
    }
}