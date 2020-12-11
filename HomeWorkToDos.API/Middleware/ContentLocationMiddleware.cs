using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Middleware
{
    /// <summary>
    /// Content Location Middleware
    /// </summary>
    public class ContentLocationMiddleware
    {

        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentLocationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ContentLocationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            loggerFactory.CreateLogger<ContentLocationMiddleware>();
            _next = next;
        }

        /// <summary>
        /// Process request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// Returns nothing.
        /// </returns>
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                int responseStatusCode = context.Response.StatusCode;
                if (responseStatusCode == (int)HttpStatusCode.Created)
                {
                    IHeaderDictionary headers = context.Response.Headers;
                    StringValues locationHeaderValue = string.Empty;
                    if (headers.TryGetValue("Content-Location", out locationHeaderValue))
                    {
                        context.Response.Headers.Remove("Content-Location");
                        context.Response.Headers.Add("Content-Location", context.Response.Headers["Location"]);
                    }
                    else
                    {
                        context.Response.Headers.Add("Content-Location", context.Response.Headers["Location"]);
                    }
                }
                return Task.FromResult(0);
            });
            await _next(context);
        }
    }
    /// <summary>
    /// Extension of application builder for exception middleware.
    /// </summary>
    public static class ContentLocationMiddlewareExtensions
    {
        /// <summary>
        /// Configure Content-Location middleware.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseContentLocationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ContentLocationMiddleware>();
        }

    }
}