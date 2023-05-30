using BetAPI.Models;
using BetAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BetAPI.Handlers
{
    public class DBKeyJWTValidationHandler : JwtSecurityTokenHandler, ISecurityTokenValidator
    {
        private readonly string _jwtSalt;
        public DBKeyJWTValidationHandler(string jwtSalt) {
            _jwtSalt = jwtSalt;
        }
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            JwtSecurityToken incomingToken = ReadJwtToken(token);

            string jwtSecret = incomingToken
                .Claims
                .First(claim => claim.Type == "jwtSecret")
                .Value;

            SecurityKey publicKeyForExternalSystem = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret + _jwtSalt));

            validationParameters.IssuerSigningKey = publicKeyForExternalSystem;

            return base.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}
