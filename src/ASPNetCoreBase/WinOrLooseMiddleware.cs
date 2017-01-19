using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreBase
{
    public class WinOrLooseMiddleware
    {
        readonly RequestDelegate _next;

        public WinOrLooseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Environment.TickCount % 2 == 0)
            {
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
