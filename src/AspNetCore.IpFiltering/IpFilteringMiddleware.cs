using System.Linq;
using System.Threading.Tasks;
using AspNetCore.IpFiltering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Whitelist
{
    public class IpFilteringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpFilteringMiddleware> _logger;
        private readonly IIpAddressValidator _addressValidator;
        private readonly IpFilteringOptions _options;
        private readonly IIpAddressResultCache _ipAddressResultCache;
        
        public IpFilteringMiddleware(RequestDelegate next,
            ILogger<IpFilteringMiddleware> logger,
            IIpAddressValidator addressValidator,
            IpFilteringOptions options,
            IIpAddressResultCache ipAddressResultCache)
        {
            _addressValidator = addressValidator;
            _next = next;
            _logger = logger;
            _options = options;
            _ipAddressResultCache = ipAddressResultCache;
        }

        public async Task Invoke(HttpContext context)
        {
            var xOriginalFor = context.Request.Headers["X-Original-For"];            
            var remoteIp = context.Connection.RemoteIpAddress;
            
            _logger.LogWarning("Request from remote IP: {ClientIP}. X-Original-For: {ProxyIP}", remoteIp,xOriginalFor.FirstOrDefault());

            var cachedAllowAccess = await _ipAddressResultCache.AllowAddress(remoteIp);

            var allowAccess = cachedAllowAccess ?? await _addressValidator.AllowAccess(remoteIp);
            
            if (!cachedAllowAccess.HasValue)
            {
                await _ipAddressResultCache.SaveResult(remoteIp, allowAccess);
            }

            if (!allowAccess)
            {
                _logger.LogWarning("Forbidden request from IP: {IP} ", remoteIp);
                context.Response.StatusCode = _options.FailureHttpStatusCode;
                await context.Response.WriteAsync(_options.FailureMessage);
                return;
            }

            await _next.Invoke(context);
        }
    }
}