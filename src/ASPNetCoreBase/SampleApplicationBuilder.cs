using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreBase
{

    public class SampleApplicationBuilder
    {
        readonly IList<Func<RequestDelegate, RequestDelegate>> _components = new List<Func<RequestDelegate, RequestDelegate>>();

        /// <summary>
        /// Adds a middleware delegate to the application's request pipeline.
        /// </summary>
        /// <param name="middleware">The middleware delegate.</param>
        /// <returns>This SampleApplicationBuilder.</returns>
        public SampleApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }

        /// <summary>
        /// Adds a middleware delagate defined in-line to the application's request pipeline.
        /// </summary>
        /// <param name="middleware">A function that handles the request or calls the given next function.</param>
        /// <returns>This SampleApplicationBuilder.</returns>
        public SampleApplicationBuilder Use(Func<HttpContext, Func<Task>, Task> middleware)
        {
            return Use( next =>
            {
                return context =>
                {
                    Func<Task> simpleNext = () => next(context);
                    return middleware(context, simpleNext);
                };
            });
        }

        /// <summary>
        /// Adds a terminal middleware delegate to the application's request pipeline.
        /// </summary>
        /// <param name="handler">A delegate that handles the request.</param>
        public void Run( RequestDelegate handler )
        {
            Use( _ => handler );
        }

        /// <summary>
        /// Builds the delegate used by this application to process HTTP requests.
        /// </summary>
        /// <returns>The request handling delegate.</returns>
        public RequestDelegate Build()
        {
            RequestDelegate app = context =>
            {
                context.Response.StatusCode = 404;
                return Task.CompletedTask;
            };
            foreach (var component in _components.Reverse())
            {
                app = component(app);
            }
            return app;
        }

    }

}
