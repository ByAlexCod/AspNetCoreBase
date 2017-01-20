using ASPNetCoreBase;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// IApplicationBuilder extensions for the WinOrLooseMiddleware.
    /// </summary>
    public static class WinOrLooseMiddlewareExtension
    {
        /// <summary>
        /// Adds the WinOrLooseMiddleware to the pipeline
        /// </summary>
        /// <param name="app">This application builder.</param>
        /// <returns>This application builder.</returns>
        public static IApplicationBuilder UseWinOrLoose(this IApplicationBuilder app)
        {
            return app.UseMiddleware<WinOrLooseMiddleware>();
        }

    }
}