using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreBase
{
    public class Startup
    {
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddScoped<IWinOrLoseService, DefaultWinOrLoseService>();
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<WinOrLoseMiddleware>();
            app.Run( async context => await context.Response.WriteAsync( "Hello World!" ) );
        }
    }
}
