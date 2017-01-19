using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASPNetCoreBase
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use( WinOrLoose );

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        async Task WinOrLoose( HttpContext ctx, Func<Task> nextMiddleware )
        {
            if( Environment.TickCount % 2 == 0 )
            {
                await ctx.Response.WriteAsync("LOOSE!");
            }
            else
            {
                await ctx.Response.WriteAsync("OK, you win! => ");
                await nextMiddleware();
            }
        }

    }


}
