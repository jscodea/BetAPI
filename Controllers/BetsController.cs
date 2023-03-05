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
using PagedList;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace BetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetsController(IBetService betService)
        {
            _betService = betService;
        }

        // GET: api/Bets
        [HttpGet]
        public async Task<IPagedList<BetDTO>> GetBet([FromQuery]PagingParameters pagingParams)
        {
            IPagedList<BetDTO> bets = await _betService.GetBetsPagedAsync(pagingParams.PageNumber, pagingParams.PageSize);

            PagingMetadata metadata = new PagingMetadata
            {
                Count = bets.Count,
                PageSize = bets.PageSize,
                PageNumber = bets.PageNumber,
                PageCount = bets.PageCount,
                HasNextPage = bets.HasNextPage,
                HasPreviousPage = bets.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return bets;
        }

        // GET: api/Bets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BetDTO>> GetBet(int id)
        {
            var bet = await _betService.GetBetAsync(id);

            if (bet == null)
            {
                return NotFound();
            }

            return bet;
        }

        // PUT: api/Bets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBet(int id, BetPutDTO bet)
        {
            int status = await _betService.UpdateBetAsync(id, bet);
            if (status == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Bets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBet(Bet bet)
        {
            await _betService.InsertBetAsync(bet);
            return NoContent();
        }

        // POST: api/BetsPlace
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Place")]
        public async Task<IActionResult> PostBetPlace(BetPlaceDTO bet)
        {
            if (!await _betService.BetPlaceAsync(bet))
            {
                return BadRequest();
            }
            return NoContent();
        }

        // PUT: api/Bets/DebugSettle/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("DebugSettle/{id}")]
        public async Task<IActionResult> PutDebugSettle(int id, decimal payout)
        {
            bool result = await _betService.BetSettleAsync(id, payout);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
