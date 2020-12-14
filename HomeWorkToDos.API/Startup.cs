using HomeWorkToDos.API.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using CorrelationId;
using AutoMapper;
using System.Linq;
using HomeWorkToDos.API.Mapper;
using HomeWorkToDos.API.Middleware;
using HomeWorkToDos.Util.ConfigModels;
using HomeWorkToDos.API.ServiceExtensions;
using HomeWorkToDos.Util.Constants;
using Microsoft.Extensions.Logging;
using HotChocolate.AspNetCore;
using Swashbuckle.AspNetCore.Filters;
using HomeWorkToDos.API.Filter;

namespace HomeWorkToDos.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtTokenConfig>(Configuration.GetSection("JwtToken"));

            services.AddControllers(p => p.RespectBrowserAcceptHeader = true).AddXmlDataContractSerializerFormatters()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddHttpContextAccessor();
            services.AddAutoMapper(c => c.AddProfile<MappingProfile>(), typeof(Startup));
            //configure services for checking, logging and forwarding correlationID.
            services.AddCorrelationIdHandlerAndDefaults();
            services.AddTokenAuthentication(Configuration);

            var connectionString = Configuration.GetConnectionString("Default");
            services.RegisterDI(connectionString);
            services.AddSwaggerExamplesFromAssemblyOf<JsonPatchPersonRequestExample>();

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CommonConstants.AllowAllCors,
                                  builder =>
                                  {
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyOrigin();
                                  });
            });
            services.AddGraphQLServices();
            services.AddControllersWithViews(options =>
                    {
                        options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                    })
                   .AddNewtonsoftJson();

            //Configure Swagger services.
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(CommonConstants.LogFile, isJson: true);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationId();
            app.UseContentLocationMiddleware();
            app.UseRequestResponseLogging();
            app.ConfigureExceptionMiddleware();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseTokenMiddleware();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGraphQL().UsePlayground();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeWork ToDos API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        //This method gets NewtonsoftJsonPatchInputFormatter for formatting JsonPatch document input.
        /// <summary>
        /// Gets the json patch input formatter.
        /// </summary>
        /// <returns></returns>
        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            ServiceProvider builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}
