using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Handler
{
    /// <summary>
    /// Implements method for forwarding CorrelationID in HttpRequestheader.
    /// </summary>
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<CorrelationIdDelegatingHandler> _logger;

        public CorrelationIdDelegatingHandler(ILogger<CorrelationIdDelegatingHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Forwards CorrelationID in HttpRequestheader.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.TryGetValues("X-Correlation-Id", out IEnumerable<string> headerEnumerable))
            {
                _logger.LogInformation("Request has the following correlation ID header {CorrelationId}.", headerEnumerable.FirstOrDefault());
            }
            else
            {
                _logger.LogInformation("Request does not have a correlation ID header.");
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            return Task.FromResult(response);
        }
    }
}

