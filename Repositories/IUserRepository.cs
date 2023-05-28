using BetAPI.DTO;
using BetAPI.Models;
using PagedList;

namespace BetAPI.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<IPagedList<UserDTO>> GetAllPagedWithInfoAsync(int pageNumber, int pageSize = 50);
        Task<List<UserDTO>> GetAllWithInfoAsync();
        Task<UserDTO?> GetByIdWithInfoAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task UpdateBalanceAsync(int id, decimal change);
        Task UpdateFromDTOAsync(int id, UserPutDTO user);
    }
}