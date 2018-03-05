using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
            services.AddOptions()
                    .AddAuthentication( "SuperCookie" )
                    .AddCookie( "SuperCookie", options =>
                    {
                        options.SlidingExpiration = true;
                        options.ExpireTimeSpan = TimeSpan.FromHours( 1 );
                    } );
            services.Configure<WinOrLooseOptions>( _configuration.GetSection( "WinOrLoose" ) );
        }


        public void Configure( IApplicationBuilder app, IHostingEnvironment env )
        {
            if( env.IsDevelopment() ) app.UseDeveloperExceptionPage();

            app.Use( async ( context, next ) =>
            {
                if( context.Request.Path.StartsWithSegments( "/login" ) )
                {
                    var identity = new ClaimsIdentity();
                    identity.AddClaim( new Claim( "name", "toto" ) );
                    ClaimsPrincipal principal = new ClaimsPrincipal( identity );
                    await context.SignInAsync( "SuperCookie", principal );
                }
                else if( context.Request.Path.StartsWithSegments( "/logout" ) )
                {
                    await context.SignOutAsync( "SuperCookie" );
                }
                await next();
            } );

app.Use( async ( ctx, next ) =>
{
    AuthenticateResult result = await ctx.AuthenticateAsync( "SuperCookie" );
    if( result.Succeeded )
    {
        string name = result
                        .Principal
                        .Identities
                        .First()
                        .Claims
                        .Single( c => c.Type == "name" )
                        .Value;
        await ctx.Response.WriteAsync( $"Hello {name}{Environment.NewLine}" );
    }
    else
    {
        await ctx.Response.WriteAsync( "Who the hell are you?" + Environment.NewLine );
    }
    await next();
} );

            app.Run( async context => await context.Response.WriteAsync( "I'm the endware!" ) );
        }
    }
}
