using BetAPI.DTO;
using BetAPI.Models;
using PagedList;

namespace BetAPI.Repositories
{
    public interface IBetRepository: IGenericRepository<Bet>
    {
        Task<IPagedList<BetDTO>> GetAllPagedWithInfoAsync(int pageNumber, int pageSize = 50);
        Task<List<BetDTO>> GetAllWithInfoAsync();
        Task<BetDTO?> GetByIdWithInfoAsync(int id);
        Task UpdateFromDTOAsync(int id, BetPutDTO bet);

        Task<List<BetDTO>> GetBetsByEventIdAsync(int eventId);
    }
}