using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Models;
using BetAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using PagedList;
using BetAPI.Repositories;
using NuGet.Protocol.Core.Types;

namespace BetAPI.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EventDTO>> GetEventsAsync()
        {
            return await _repository.GetAllWithInfoAsync();
        }

        public async Task<IPagedList<EventDTO>> GetEventsPagedAsync(int pageNumber, int pageSize = 50)
        {
            return await _repository.GetAllPagedWithInfoAsync(pageNumber, pageSize);
        }

        public async Task<EventDTO?> GetEventAsync(int id)
        {
            return await _repository.GetByIdWithInfoAsync(id);
        }

        public async Task<int> InsertEventAsync(Event ev)
        {
            await _repository.InsertAsync(ev);
            return await _repository.SaveAsync();
        }

        public async Task<int> UpdateEventAsync(int id, EventPutDTO ev)
        {
            await _repository.UpdateFromDTOAsync(id, ev);
            return await _repository.SaveAsync();
        }

        public async Task<int> SetEventResult(int id, int Result)
        {
            Event? existingEvent = await _repository.GetByIdAsync(id);
            if (existingEvent == null)
            {
                throw new EventUnavailableException("Event does not exist for resulting.");
            }
            existingEvent.Result = Result;
            return await _repository.SaveAsync();
        }
    }
}
