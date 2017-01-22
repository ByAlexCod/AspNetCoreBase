using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace ASPNetCoreBase
{

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<WinOrLooseOptions>(Configuration.GetSection("WinOrLoose"));
            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "TheMarvellousCookie"
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/login"))
                {
                    var identity = new ClaimsIdentity();
                    identity.AddClaim(new Claim("name", "toto"));
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await context.Authentication.SignInAsync("TheMarvellousCookie", principal);
                }
                else if (context.Request.Path.StartsWithSegments("/logout"))
                {
                    await context.Authentication.SignOutAsync("TheMarvellousCookie");
                }
                await next();
            });

            app.Use(async (context, next) =>
            {
                ClaimsPrincipal principal = await context.Authentication.AuthenticateAsync("TheMarvellousCookie");
                string name = principal == null 
                                ? "Anonymous" 
                                : principal.Identities.First().Claims.Single(c => c.Type == "name").Value;
                await context.Response.WriteAsync($"Hello {name}{Environment.NewLine}" );
                await next();
            });

            app.UseWinOrLoose();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });

        }

    }


}
