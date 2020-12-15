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
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CorrelationIdDelegatingHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdDelegatingHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CorrelationIdDelegatingHandler(ILogger<CorrelationIdDelegatingHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Forwards CorrelationID in HttpRequestheader.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
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

