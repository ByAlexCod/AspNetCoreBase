using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AspNetCoreBase
{
    public class DefaultWinOrLoseService : IWinOrLoseService
    {
        readonly ILogger _logger;

        public DefaultWinOrLoseService( ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DefaultWinOrLoseService>();
            _logger.LogError( "CAAALLLIIINNNGGG the DefaultWinOrLoseService CONSTRUCTOR." );
        }

        public Task<bool> WinOrLoseAsync( HttpContext c )
        {
            bool win = Environment.TickCount % 2 == 0;
            return Task.FromResult( win );
        }
    }
}
