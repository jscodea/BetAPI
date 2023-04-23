using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Helpers;
using BetAPI.Exceptions;
using BetAPI.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BetAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly BetAPIContext _context;
        private readonly IUserService _userService;

        public AuthenticationService(BetAPIContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
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
            if (! isValid)
            {
                //TODO should throw same exception as bad username to not giveaway info
                throw new UserUnavailableException("Bad password");
            }

            //TODO generate JWT token and sign with user salt and return it
            //array of data, algorythm name, salt

            return isValid;
        }
    }
}
