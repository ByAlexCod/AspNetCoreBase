using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreBase
{
    public class WinOrLoseMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger _log;

        public WinOrLoseMiddleware(
            RequestDelegate next,
            ILoggerFactory log )
        {
            _next = next;
            _log = log.CreateLogger<WinOrLoseMiddleware>();
        }

        public async Task Invoke( HttpContext context, IWinOrLoseService service )
        {
            if( await service.WinOrLoseAsync( context ) )
            {
                _log.LogWarning( "Lost." );
                await context.Response.WriteAsync( "LOOSE!" );
            }
            else
            {
                await context.Response.WriteAsync( "OK, you win! =>"  );
                await _next( context );
            }
        }
    }

}
