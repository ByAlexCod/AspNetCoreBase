using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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

        public WinOrLooseMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<WinOrLooseMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (Environment.TickCount % 2 == 0)
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
