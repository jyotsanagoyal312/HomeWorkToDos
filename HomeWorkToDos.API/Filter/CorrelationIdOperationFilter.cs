using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace HomeWorkToDos.API.Filter
{
    /// <summary>
    /// Add CorrelationId header parameters.
    /// </summary>
    /// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter" />
    public class CorrelationIdOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Custom-Correlation-Id",
                In = ParameterLocation.Header,
                Description = "Id to track particular request/response",
                Required = false
            });
        }
    }
}
