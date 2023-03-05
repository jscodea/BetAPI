using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetAPI.Data;
using BetAPI.Models;
using BetAPI.DTO;
using BetAPI.Services;
using Newtonsoft.Json;
using PagedList;

namespace BetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<IPagedList<EventDTO>> GetEvent([FromQuery]PagingParameters pagingParams)
        {
            IPagedList<EventDTO> events = await _eventService.GetEventsPagedAsync(pagingParams.PageNumber, pagingParams.PageSize);

            PagingMetadata metadata = new PagingMetadata
            {
                Count = events.Count,
                PageSize = events.PageSize,
                PageNumber = events.PageNumber,
                PageCount = events.PageCount,
                HasNextPage = events.HasNextPage,
                HasPreviousPage = events.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return events;
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var ev = await _eventService.GetEventAsync(id);

            if (ev == null)
            {
                return NotFound();
            }

            return ev;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventPutDTO ev)
        {
            int status = await _eventService.UpdateEventAsync(id, ev);
            if (status == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBet(Event ev)
        {
            await _eventService.InsertEventAsync(ev);
            return NoContent();
        }
    }
}
