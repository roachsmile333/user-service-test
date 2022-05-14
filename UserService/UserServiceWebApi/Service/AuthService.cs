using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.Models.User;
using UserServiceWebApi.Helpers;

namespace UserServiceWebApi.Service
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Method for authentication where Token as result in success
        /// </summary>
        /// <param name="user">User model object</param>
        /// <returns>Token object</returns>
        public Token Authenticate(UserViewModel user)
        {
            var jwtSection = _configuration.GetSection("JWTConfig");
            //searching target data in db
            if (user.UserName != jwtSection.GetSection("Username").Value || user.Password != jwtSection.GetSection("Password").Value)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName)
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            var tokenHandler = new JwtSecurityTokenHandler();
            DateTime dateTimeNow = DateTime.Now;
            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: jwtSection.GetSection("Issuer").Value,
                audience: jwtSection.GetSection("Audience").Value,
                notBefore: dateTimeNow,
                claims: claimsIdentity.Claims,
                expires: dateTimeNow.Add(TimeSpan.FromMinutes(Double.Parse(jwtSection.GetSection("Lifetime").Value))),
                signingCredentials: new SigningCredentials(
                    key: AuthServiceHelper.GetKey(jwtSection.GetSection("Key").Value), 
                    algorithm: SecurityAlgorithms.HmacSha256Signature
                )
            );
            var token = tokenHandler.WriteToken(tokenDescriptor);
            var result = new Token
            {
                OriginToken = token
            };
            return result;
        }
    }
}
