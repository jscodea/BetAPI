using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Helpers;
using BetAPI.Models;
using BetAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PagedList;
using BetAPI.Repositories;

namespace BetAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IAppCache _appCache;

        public UserService(IUserRepository repository, IAppCache appCache)
        {
            _repository = repository;
            _appCache = appCache;
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            return await _repository.GetAllWithInfoAsync();
        }

        public async Task<IPagedList<UserDTO>> GetUsersPagedAsync(int pageNumber, int pageSize = 50)
        {
            return await _repository.GetAllPagedWithInfoAsync(pageNumber, pageSize);
        }

        public async Task<UserDTO?> GetUserAsync(int id)
        {
            return await _repository.GetByIdWithInfoAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _repository.GetUserByUsernameAsync(username);
        }

        public async Task<int> InsertUserAsync(User user)
        {
            await _repository.InsertAsync(user);
            return await _repository.SaveAsync();
        }

        public async Task<int> UpdateUserAsync(int id, UserPutDTO user)
        {
            await _repository.UpdateFromDTOAsync(id, user);
            return await _repository.SaveAsync();
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
            await _repository.InsertAsync(user);
            return await _repository.SaveAsync();
        }

        public async Task<int> UpdateBalance(int id, decimal change)
        {
            User? existingUser = await _repository.GetByIdAsync(id);
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

            int balanceUpdated = await _repository.SaveAsync();

            await SetBalanceUpdate(id, false);

            return balanceUpdated;
        }

        private async Task<bool> SetBalanceUpdate(int id, bool setUpdating)
        {
            User? existingUser = await _repository.GetByIdAsync(id);
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
