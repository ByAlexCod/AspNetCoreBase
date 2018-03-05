using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreBase
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup( IConfiguration configuration )
        {
            _configuration = configuration;

            // Direct use of configuration is possible.
            // But Option objects should be preferred.
            IConfigurationSection section = _configuration.GetSection( "WinOrLoose" );
            string oneOutOf = section["OneOutOf"];
            string shorterWay = _configuration["WinOrLoose:OneOutOf"];
            int typed = _configuration.GetValue<int>("WinOrLoose:OneOutOf");
        }

        public void ConfigureServices( IServiceCollection services )
        {
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            app.Run( async context => await context.Response.WriteAsync( "Hello World!" ) );
        }
    }
}
