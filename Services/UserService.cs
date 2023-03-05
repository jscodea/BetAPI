using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Models;
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

        public async Task<int> UpdateBalance(int id, decimal stake)
        {
            User? existingUser = await _context.User.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser == null || BalanceIsUpdating(existingUser.BalanceUpdateStarted))
            {
                return 0;
            }

            int updateSet = await SetBalanceUpdate(id, true);

            if (updateSet == 0)
            {
                return 0;
            }

            existingUser.Balance = existingUser.Balance + stake;

            int balanceUpdated = await _context.SaveChangesAsync();

            await SetBalanceUpdate(id, false);

            return balanceUpdated;
        }

        private async Task<int> SetBalanceUpdate(int id, bool setUpdating)
        {
            User? existingUser = await _context.User.FirstOrDefaultAsync(i => i.Id == id);
            if (existingUser == null)
            {
                return 0;
            }
            if (setUpdating && _appCache.Cache.TryGetValue(id, out int id))
            {
                return 0;
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60));

            _memoryCache.Set(CacheKeys.Entry, cacheValue, cacheEntryOptions);
            existingUser.BalanceUpdateStarted = setUpdating ? DateTime.Now : null;
            return await _context.SaveChangesAsync();
        }
    }
}
