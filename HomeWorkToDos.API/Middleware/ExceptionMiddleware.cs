using CorrelationId.Abstractions;
using HomeWorkToDos.Util.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Middleware
{
    /// <summary>
    /// Exception Middleware
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// The correlation context accessor
        /// </summary>
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="correlationContextAccessor">The correlation context accessor.</param>
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, ICorrelationContextAccessor correlationContextAccessor)
        {
            this._logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
            this._next = next;
            this._correlationContextAccessor = correlationContextAccessor;
        }

        /// <summary>
        /// Process request.
        /// </summary>
        /// <param name="httpContext">HttpContext.</param>
        /// <returns>
        /// Returns nothing.
        /// </returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        /// <summary>
        /// Handles exception and adds message in response with status code .
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string correlationId = _correlationContextAccessor.CorrelationContext.CorrelationId;
            _logger.LogError($"Error: {exception}, Correlation id: {correlationId}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the exception middleware."
            }.ToString());
        }
    }
    /// <summary>
    /// Extension of application builder for exception middleware.
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Configure exception middleware.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }

    }
}

