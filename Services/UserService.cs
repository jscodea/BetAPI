using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Helpers;
using BetAPI.Models;
using BetAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PagedList;

namespace BetAPI.Services
{
    public class UserService : IUserService
    {
        private readonly BetAPIContext _context;
        private readonly IAppCache _appCache;

        public UserService(BetAPIContext context, IAppCache appCache)
        {
            _context = context;
            _appCache = appCache;
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            List<UserDTO> users = await _context.User.Include(p => p.Bets).Select(
                  s => new UserDTO().SetFromUser(s)
              ).ToListAsync();

            return users;
        }

        public async Task<IPagedList<UserDTO>> GetUsersPagedAsync(int pageNumber, int pageSize = 50)
        {
            IPagedList<UserDTO> usersPaged = await PagedListExtendedExtensions.ToPagedListAsync<UserDTO>(_context.User.Include(p => p.Bets).Select(
                  s => new UserDTO().SetFromUser(s)
              ), pageNumber, pageSize);

            return usersPaged;
        }

        public async Task<UserDTO?> GetUserAsync(int id)
        {
            User? user = await _context.User.Include(p => p.Bets).FirstOrDefaultAsync(i => i.Id == id);

            return user != null ? new UserDTO().SetFromUser(user) : null;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.User.Include(p => p.Bets).FirstOrDefaultAsync(i => i.Username == username);
        }

        public async Task<int> InsertUserAsync(User user)
        {
            await _context.User.AddAsync(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateUserAsync(int id, UserPutDTO user)
        {
            User? existingUser = await _context.User.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser == null)
            {
                return 0;
            }
            if (user.IsActive != null)
            {
                existingUser.IsActive = (bool) user.IsActive;
            }
            if (user.Balance != null)
            {
                existingUser.Balance = (decimal) user.Balance;
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RegisterUser(string Username, string Password, string Firstname, string Lastname)
        {
            string HashedPassword = PasswordHelper.HashPasword(Password, out var salt);
            User user = new User
            {
                Username = Username,
                Password = HashedPassword,
                FirstName = Firstname,
                LastName = Lastname,
                Salt = salt,
                IsActive = true,
                Balance = 0
            };
            await _context.User.AddAsync(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateBalance(int id, decimal change)
        {
            User? existingUser = await _context.User.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser == null || IsUpdating(id))
            {
                return 0;
            }

            bool updateSet = await SetBalanceUpdate(id, true);
            if (!updateSet)
            {
                throw new GenericException("Balance is already being updated.");
            }

            existingUser.Balance = existingUser.Balance + change;

            int balanceUpdated = await _context.SaveChangesAsync();

            await SetBalanceUpdate(id, false);

            return balanceUpdated;
        }

        private async Task<bool> SetBalanceUpdate(int id, bool setUpdating)
        {
            User? existingUser = await _context.User.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser == null)
            {
                return false;
            }
            if (setUpdating && IsUpdating(id))
            {
                return false;
            }
            if (setUpdating)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60)).SetSize(1);

                _appCache.Cache.Set(id, DateTime.Now, cacheEntryOptions);

                return true;
            }

            _appCache.Cache.Remove(id);

            return true;
        }

        private bool IsUpdating(int id)
        {
            return _appCache.Cache.TryGetValue(id, out DateTime cacheResult);
        }
    }
}
