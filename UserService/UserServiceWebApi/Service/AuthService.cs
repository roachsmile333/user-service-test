using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserServiceWebApi.Helpers;
using UserServiceWebApi.Models;

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
        public Token Authenticate(User user)
        {
            //searching target data in db
            if(user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    key: AuthServiceHelper.GetKey(_configuration.GetSection("JWTConfig").GetSection("Key").Value), 
                    algorithm: SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new Token
            {
                OriginToken = tokenHandler.WriteToken(token)
            };
            return result;
        }
    }
}
