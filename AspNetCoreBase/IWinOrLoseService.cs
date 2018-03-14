using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreBase
{
    public interface IWinOrLoseService
    {
        Task<bool> WinOrLoseAsync( HttpContext c );

    }
}
