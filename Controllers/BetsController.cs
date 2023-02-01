using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetAPI.Data;
using BetAPI.Models;

namespace BetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly BetAPIContext _context;

        public BetsController(BetAPIContext context)
        {
            _context = context;
        }

        // GET: api/Bets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bet>>> GetBet()
        {
          if (_context.Bet == null)
          {
              return NotFound();
          }
            return await _context.Bet.ToListAsync();
        }

        // GET: api/Bets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bet>> GetBet(int id)
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
        public async Task<ActionResult<Bet>> PostBet(Bet bet)
        {
          if (_context.Bet == null)
          {
              return Problem("Entity set 'BetAPIContext.Bet'  is null.");
          }
            _context.Bet.Add(bet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBet", new { id = bet.Id }, bet);
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
