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

        public WhiteListMiddleware(RequestDelegate next,
            ILogger<WhiteListMiddleware> logger,
            IIpAddressValidator addressValidator,
            WhitelistOptions options)
        {
            _addressValidator = addressValidator;
            _next = next;
            _logger = logger;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            _logger.LogWarning("Request from remote IP: {IP}", remoteIp);

            if (!await _addressValidator.AllowAccess(remoteIp))
            {
                _logger.LogWarning("Forbidden request from IP: {IP} ", remoteIp);
                context.Response.StatusCode = _options.FailureHttpStatusCode;
                return;
            }

            await _next.Invoke(context);
        }
    }
}