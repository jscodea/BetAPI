using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Helpers;
using BetAPI.Exceptions;
using BetAPI.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Caching.Memory;

namespace BetAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserService userService, ILogger<AuthenticationService> logger, IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
        }

        //Todo some JWT and session logic should be done
        //use user service to get user
        public async Task<bool> LoginAsync(string username, string password)
        {
            User? user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                throw new UserUnavailableException("User with that username does not exist");
            }

            bool isValid = PasswordHelper.VerifyPassword(password, user.Password, user.Salt);
            if (!isValid)
            {
                //TODO should throw same exception as bad username to not giveaway info
                throw new UserUnavailableException("Bad password");
            }
            string jwtToken = GenerateJWT(user);
            _logger.LogInformation("Generated JWT {0}", jwtToken);

            return isValid;
        }

        private string GenerateJWT(User user, int expireMinutes = 30)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.JWTSecret+_configuration.GetValue<string>("JwtSalt")));
            //var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(user.JWTSecret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.Id.ToString(), ClaimValueTypes.String),
                    new Claim("jwtSecret", user.JWTSecret, ClaimValueTypes.String)
                }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(
                    symmetricKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;

        }
    }
}
