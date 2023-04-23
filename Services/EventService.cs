using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Models;
using BetAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace BetAPI.Services
{
    public class EventService : IEventService
    {
        private readonly BetAPIContext _context;

        public EventService(BetAPIContext context)
        {
            _context = context;
        }

        public async Task<List<EventDTO>> GetEventsAsync()
        {
            List<EventDTO> events = await _context.Event.Select(
                  s => new EventDTO().SetFromEvent(s)
              ).ToListAsync();

            return events;
        }

        public async Task<IPagedList<EventDTO>> GetEventsPagedAsync(int pageNumber, int pageSize = 50)
        {
            IPagedList<EventDTO> eventsPaged = await PagedListExtendedExtensions.ToPagedListAsync<EventDTO>(_context.Event.Select(
                  s => new EventDTO().SetFromEvent(s)
              ), pageNumber, pageSize);

            return eventsPaged;
        }

        public async Task<EventDTO?> GetEventAsync(int id)
        {
            Event? ev = await _context.Event.FirstOrDefaultAsync(i => i.Id == id);

            return ev != null ? new EventDTO().SetFromEvent(ev) : null;
        }

        public async Task<int> InsertEventAsync(Event ev)
        {
            await _context.Event.AddAsync(ev);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateEventAsync(int id, EventPutDTO ev)
        {
            Event? existingEvent = await _context.Event.FirstOrDefaultAsync(i => i.Id == id);
            if (existingEvent == null)
            {
                return 0;
            }
            if(ev.Opt1 != null)
            {
                existingEvent.Opt1 = (decimal) ev.Opt1;
            }
            if (ev.Opt2 != null)
            {
                existingEvent.Opt2 = (decimal) ev.Opt2;
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SetEventResult(int id, int Result)
        {
            Event? existingEvent = await _context.Event.FirstOrDefaultAsync(i => i.Id == id);
            if (existingEvent == null)
            {
                throw new EventUnavailableException("Event does not exist for resulting.");
            }
            existingEvent.Result = Result;
            return await _context.SaveChangesAsync();
        }
    }
}
