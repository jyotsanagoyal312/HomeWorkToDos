using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Middleware
{
    /// <summary>
    /// Logging Middleware
    /// </summary>
    public class LoggingMiddleware
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
        /// The recyclable memory stream manager
        /// </summary>
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        /// <summary>
        /// The correlation context accessor
        /// </summary>
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// Create new instance of <see cref="LoggingMiddleware" /> class.
        /// </summary>
        /// <param name="next">Next request delegate.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="correlationContextAccessor">CorrelationContext accessor.</param>
        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, ICorrelationContextAccessor correlationContextAccessor)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _correlationContextAccessor = correlationContextAccessor;
        }

        /// <summary>
        /// Process request.
        /// </summary>
        /// <param name="context">Http context.</param>
        /// <returns>
        /// Returns nothing.
        /// </returns>
        public async Task Invoke(HttpContext context)
        {
            string correlationId = _correlationContextAccessor.CorrelationContext.CorrelationId;
            await LogRequest(context, correlationId);
            await LogResponse(context, correlationId);
        }

        /// <summary>
        /// Logs the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        private async Task LogRequest(HttpContext context, string correlationId)
        {
            context.Request.EnableBuffering();

            await using MemoryStream requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Request Body: {ReadStreamInChunks(requestStream)}" +
                                   $"Correlation Id: {correlationId}");
            context.Request.Body.Position = 0;
        }

        /// <summary>
        /// Logs the response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        private async Task LogResponse(HttpContext context, string correlationId)
        {
            Stream originalBodyStream = context.Response.Body;

            await using MemoryStream responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(stream: context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Response Body: {text}" +
                                   $"Correlation Id: {correlationId}");

            await responseBody.CopyToAsync(originalBodyStream);
        }

        /// <summary>
        /// Reads the stream in chunks.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using StringWriter textWriter = new StringWriter();
            using StreamReader reader = new StreamReader(stream);

            char[] readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
    /// <summary>
    /// Extension of application builder for request response logging middleware.
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Configure request response logging middleware
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <returns>
        /// Returns application builder.
        /// </returns>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}

