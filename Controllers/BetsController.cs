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

namespace BetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly BetAPIContext _context;
        private readonly IBetService _betService;

        public BetsController(BetAPIContext context, IBetService betService)
        {
            _context = context;
            _betService = betService;
        }

        // GET: api/Bets
        [HttpGet]
        public async Task<ActionResult<List<BetDTO>>> GetBet()
        {
            return await _betService.GetBetsAsync();
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
        public async Task<IActionResult> PutBet(int id, Bet bet)
        {
            if (id != bet.Id)
            {
                return BadRequest();
            }

            _context.Entry(bet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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

        // DELETE: api/Bets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBet(int id)
        {
            if (_context.Bet == null)
            {
                return NotFound();
            }
            var bet = await _context.Bet.FindAsync(id);
            if (bet == null)
            {
                return NotFound();
            }

            _context.Bet.Remove(bet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BetExists(int id)
        {
            return (_context.Bet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
