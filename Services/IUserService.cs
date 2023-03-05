using BetAPI.DTO;
using BetAPI.Models;
using PagedList;

namespace BetAPI.Services
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetUsersAsync();
        Task<IPagedList<UserDTO>> GetUsersPagedAsync(int pageNumber, int pageSize);
        Task<UserDTO?> GetUserAsync(int id);
        Task<int> InsertUserAsync(User bet);
        Task<int> UpdateUserAsync(int id, UserPutDTO user);
    }
}
