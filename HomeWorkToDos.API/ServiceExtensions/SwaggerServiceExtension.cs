using HomeWorkToDos.API.Filter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HomeWorkToDos.API.ServiceExtensions
{
    /// <summary>
    /// Swagger Service Extension
    /// </summary>
    public static class SwaggerServiceExtension
    {
        /// <summary>
        /// implements extension method for adding Swagger services.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeWork ToDos API", Version = "v1" });
                p.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                p.ExampleFilters();

                p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                p.OperationFilter<CorrelationIdOperationFilter>();
                p.OperationFilter<RemoveVersionParameterFilter>();
                p.SchemaFilter<ExampleSchemaFilter>();
                p.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();

                p.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                ////Set the comments path for the Swagger JSON and UI.

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                p.IncludeXmlComments(xmlFile);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            return services;
        }
    }
}