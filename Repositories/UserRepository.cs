using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace BetAPI.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BetAPIContext _context) : base(_context)
        {
        }

        public async Task<List<UserDTO>> GetAllWithInfoAsync()
        {
            List<UserDTO> users = await _context.User.Include(p => p.Bets).Select(
                  s => new UserDTO().SetFromUser(s)
              ).ToListAsync();

            return users;
        }

        public async Task<IPagedList<UserDTO>> GetAllPagedWithInfoAsync(int pageNumber, int pageSize = 50)
        {
            IPagedList<UserDTO> usersPaged = await PagedListExtendedExtensions.ToPagedListAsync<UserDTO>(_context.User.Include(p => p.Bets).Select(
                  s => new UserDTO().SetFromUser(s)
              ), pageNumber, pageSize);

            return usersPaged;
        }

        public async Task<UserDTO?> GetByIdWithInfoAsync(int id)
        {
            User? user = await _context.User.Include(p => p.Bets).FirstOrDefaultAsync(i => i.Id == id);

            return user != null ? new UserDTO().SetFromUser(user) : null;
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.User.Include(p => p.Bets).FirstOrDefaultAsync(i => i.Username == username);
        }

        public async Task UpdateFromDTOAsync(int id, UserPutDTO user)
        {
            User? existingUser = await _context.User.FirstOrDefaultAsync(i => i.Id == id);
            if (user.IsActive != null)
            {
                existingUser.IsActive = (bool)user.IsActive;
            }
            if (user.Balance != null)
            {
                existingUser.Balance = (decimal)user.Balance;
            }
        }

        public async Task UpdateBalanceAsync(int id, decimal change)
        {
            User? existingUser = await _context.User.FirstOrDefaultAsync(i => i.Id == id);
            existingUser.Balance = existingUser.Balance + change;
        }
    }
}
