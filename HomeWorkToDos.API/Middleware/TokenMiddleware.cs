using HomeWorkToDos.Business.Contract;
using HomeWorkToDos.Util.ConfigModels;
using HomeWorkToDos.Util.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkToDos.API.Middleware
{
    /// <summary>
    /// Token Middleware
    /// </summary>
    public class TokenMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// The JWT token configuration
        /// </summary>
        private readonly JwtTokenConfig _jwtTokenConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="jwtTokenConfig">The JWT token configuration.</param>
        public TokenMiddleware(RequestDelegate next, IOptions<JwtTokenConfig> jwtTokenConfig)
        {
            this._next = next;
            this._jwtTokenConfig = jwtTokenConfig.Value;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userService">The user service.</param>
        public async Task Invoke(HttpContext context, IUser userService)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                await AttachUserToContext(context, userService, token);
            }
            await _next(context);
        }
        /// <summary>
        /// Attaches the user to context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="token">The token.</param>
        /// <exception cref="AccessViolationException"></exception>
        private async Task AttachUserToContext(HttpContext context, IUser userService, string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token = token.Replace("Bearer ", string.Empty);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtTokenConfig.Issuer,
                ValidAudience = _jwtTokenConfig.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfig.Key))
            }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

            //attach user to context on successful jwt validation
            int UserId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
            UserDto userDto = await userService.GetById(UserId);
            if (userDto == null)
            {
                throw new AccessViolationException();
            }
            context.Items["UserId"] = userDto.UserId;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class TokenMiddlewareExtension
    {
        /// <summary>
        /// Uses the token middleware.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
