using BetAPI.DTO;
using BetAPI.Models;
using PagedList;

namespace BetAPI.Repositories
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IPagedList<EventDTO>> GetAllPagedWithInfoAsync(int pageNumber, int pageSize = 50);
        Task<List<EventDTO>> GetAllWithInfoAsync();
        Task<EventDTO?> GetByIdWithInfoAsync(int id);
        Task UpdateFromDTOAsync(int id, EventPutDTO ev);
    }
}