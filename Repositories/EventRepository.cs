using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore;
using PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BetAPI.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(BetAPIContext _context) : base(_context)
        {
        }

        public async Task<List<EventDTO>> GetAllWithInfoAsync()
        {
            List<EventDTO> events = await _context.Event.Select(
                  s => new EventDTO().SetFromEvent(s)
              ).ToListAsync();

            return events;
        }

        public async Task<IPagedList<EventDTO>> GetAllPagedWithInfoAsync(int pageNumber, int pageSize = 50)
        {
            IPagedList<EventDTO> eventsPaged = await PagedListExtendedExtensions.ToPagedListAsync<EventDTO>(_context.Event.Select(
                  s => new EventDTO().SetFromEvent(s)
              ), pageNumber, pageSize);

            return eventsPaged;
        }

        public async Task<EventDTO?> GetByIdWithInfoAsync(int id)
        {
            Event? ev = await _context.Event.FirstOrDefaultAsync(i => i.Id == id);

            return ev != null ? new EventDTO().SetFromEvent(ev) : null;
        }

        public async Task<List<EventDTO>> GetEventsEndedAsync()
        {
            List<EventDTO> events = await _context.Event.Where(w => DateTime.Compare(w.EndsAt, DateTime.UtcNow) < 0).Select(
            s => new EventDTO().SetFromEvent(s)
            ).ToListAsync();

            return events;
        }

        public async Task UpdateFromDTOAsync(int id, EventPutDTO ev)
        {
            Event? existingEvent = await _context.Event.FirstOrDefaultAsync(i => i.Id == id);
            if (ev.Opt1 != null)
            {
                existingEvent.Opt1 = (decimal)ev.Opt1;
            }
            if (ev.Opt2 != null)
            {
                existingEvent.Opt2 = (decimal)ev.Opt2;
            }
        }
    }
}
