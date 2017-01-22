using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreBase
{
    public class WinOrLooseMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger _logger;
        readonly WinOrLooseOptions _options;

        public WinOrLooseMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<WinOrLooseOptions> option)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<WinOrLooseMiddleware>();
            _options = option.Value;
        }

        public async Task Invoke(HttpContext context )
        {
            await context.Response.WriteAsync($"FYI, OneOutOf: {_options.OneOutOf}{Environment.NewLine}");
            if (Environment.TickCount % _options.OneOutOf == 0)
            {
                _logger.LogWarning("Loose happened!");
                await context.Response.WriteAsync("LOOSE!");
            }
            else
            {
                await context.Response.WriteAsync("OK, you win! => ");
                await _next(context);
            }
        }
    }
}
