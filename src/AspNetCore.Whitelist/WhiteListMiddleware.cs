using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Whitelist
{
    public class WhiteListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WhiteListMiddleware> _logger;
        private readonly IIpAddressValidator _addressValidator;
        private readonly WhitelistOptions _options;
        private readonly IIpAddressResultCache _ipAddressResultCache;
        public WhiteListMiddleware(RequestDelegate next,
            ILogger<WhiteListMiddleware> logger,
            IIpAddressValidator addressValidator,
            WhitelistOptions options,
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
            
            _logger.LogWarning("Request from remote IP: {IP}. X-Original-For: {X-Original-For}", remoteIp, xOriginalFor);

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
                return;
            }

            await _next.Invoke(context);
        }
    }
}