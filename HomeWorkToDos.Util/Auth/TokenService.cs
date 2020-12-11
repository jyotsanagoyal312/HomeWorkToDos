using HomeWorkToDos.Util.ConfigModels;
using HomeWorkToDos.Util.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeWorkToDos.Util.Auth
{
    /// <summary>
    /// TokenService
    /// </summary>
    public static class TokenService
    {
        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="userDto">The user dto.</param>
        /// <param name="jwtTokenConfig">The JWT token configuration.</param>
        /// <returns></returns>
        public static string GenerateJwtToken(UserDto userDto, JwtTokenConfig jwtTokenConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Role, "User"),
                new Claim("UserId", Convert.ToString(userDto.UserId)),
                new Claim("UserName",userDto.UserName),
            };

            var token = new JwtSecurityToken(jwtTokenConfig.Issuer,
                jwtTokenConfig.Issuer,
                claims,
                DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
