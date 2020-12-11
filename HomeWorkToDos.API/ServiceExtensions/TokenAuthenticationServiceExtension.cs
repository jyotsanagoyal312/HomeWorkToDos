using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HomeWorkToDos.API.ServiceExtensions
{
    /// <summary>
    /// Token Authentication Service Extension
    /// </summary>
    public static class TokenAuthenticationServiceExtension
    {
        /// <summary>
        /// Adds the token authentication.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtToken:Issuer"],
                    ValidAudience = configuration["JwtToken:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:Key"]))
                };
            });

            return services;
        }
    }
}

