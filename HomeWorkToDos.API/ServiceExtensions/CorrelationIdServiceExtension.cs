using CorrelationId.DependencyInjection;
using CorrelationId.HttpClient;
using HomeWorkToDos.API.Handler;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HomeWorkToDos.API.ServiceExtensions
{
    /// <summary>
    /// CorrelationId Service Extension
    /// </summary>
    public static class CorrelationIdServiceExtension
    {
        /// <summary>
        /// Adds the correlation identifier handler and defaults.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddCorrelationIdHandlerAndDefaults(this IServiceCollection services)
        {
            services.AddTransient<CorrelationIdDelegatingHandler>();

            services.AddHttpClient("HomeWorkToDos_Client")
                .AddCorrelationIdForwarding()           // add the handler to attach the correlation ID to outgoing requests for this named client
                .AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

            services.AddDefaultCorrelationId(options =>
            {
                options.CorrelationIdGenerator = () => Guid.NewGuid().ToString();
                options.AddToLoggingScope = true;
                options.EnforceHeader = false;
                options.IgnoreRequestHeader = false;
                options.IncludeInResponse = true;
                options.RequestHeader = "Custom-Correlation-Id";
                options.ResponseHeader = "X-Correlation-Id";
                options.UpdateTraceIdentifier = false;
            });
            return services;
        }
    }
}
