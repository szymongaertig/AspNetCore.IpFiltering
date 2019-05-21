using Microsoft.AspNetCore.Builder;

namespace AspNetCore.Whitelist
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWhitelistMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<WhiteListMiddleware>();
            return applicationBuilder;
        }
    }
}