using BetAPI.DTO;
using BetAPI.Models;
using PagedList;

namespace BetAPI.Services
{
    public interface IEventService
    {
        Task<List<EventDTO>> GetEventsAsync();
        Task<IPagedList<EventDTO>> GetEventsPagedAsync(int pageNumber, int pageSize);
        Task<EventDTO?> GetEventAsync(int id);
        Task<List<EventDTO>> GetEventsEndedAsync();
        Task<int> InsertEventAsync(Event ev);
        Task<int> UpdateEventAsync(int id, EventPutDTO ev);
        Task<int> SetEventResult(int id, int Result);
    }
}
