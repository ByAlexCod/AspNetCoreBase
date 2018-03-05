using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace AspNetCoreBase
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var builder = new WebHostBuilder()
                .UseContentRoot( Directory.GetCurrentDirectory() )
                .ConfigureAppConfiguration( ( hostingContext, confBuilder ) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    confBuilder
                        .AddJsonFile( "appsettings.json",
                                        optional: true,
                                        reloadOnChange: true )
                        .AddJsonFile( $"appsettings.{env.EnvironmentName}.json",
                                        optional: true,
                                        reloadOnChange: true )
                        .AddEnvironmentVariables()
                        .AddCommandLine( args );
                } )
                .ConfigureLogging( ( hostingContext, logging ) =>
                    {
                        logging.AddConfiguration( hostingContext.Configuration.GetSection( "Logging" ) );
                        logging.AddConsole();
                    } )
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>();
            builder.Build().Run();
        }
    }
}
