using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore.IpFiltering
{
    public class IpFilteringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpFilteringMiddleware> _logger;
        private readonly IIpAddressValidator _addressValidator;
        private readonly IIpFilteringOptions _options;
        private readonly IIpAddressResultCache _ipAddressResultCache;

        public IpFilteringMiddleware(RequestDelegate next,
            ILogger<IpFilteringMiddleware> logger,
            IIpAddressValidator addressValidator,
            IIpFilteringOptions options,
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

            _logger.LogInformation("Request from remote IP: {ClientIP}. X-Original-For: {ProxyIP}", remoteIp,
                xOriginalFor.FirstOrDefault());

            if (ValidateWhitelist(context))
            {
                var cachedAllowAccess = await _ipAddressResultCache
                    .AllowAddress(remoteIp);

                var allowAccess = cachedAllowAccess ?? await _addressValidator.AllowAccess(remoteIp);

                if (!cachedAllowAccess.HasValue)
                {
                    await _ipAddressResultCache.SaveResult(remoteIp, allowAccess);
                }

                if (!allowAccess)
                {
                    if (_options.LearningMode)
                    {
                        _logger.LogWarning(
                            "Request from not allowed IP: {IP} has been approved because of learning mode turned ON",
                            remoteIp);
                    }
                    else
                    {
                        _logger.LogWarning("Forbidden request from IP: {IP} ", remoteIp);
                        context.Response.StatusCode = _options.FailureHttpStatusCode;
                        if (!string.IsNullOrWhiteSpace(_options.FailureMessage))
                        {
                            await context.Response.WriteAsync(_options.FailureMessage);
                        }

                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }

        private bool ValidateWhitelist(HttpContext context)
        {
            if (_options.IgnoredPaths == null)
            {
                return true;
            }

            var currentPath = context.Request.Path;
            return _options.IgnoredPaths.All(ip => !Regex.IsMatch(currentPath, ip, RegexOptions.IgnoreCase));
        }
    }
}