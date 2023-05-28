using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Generics;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore;
using PagedList;

namespace BetAPI.Repositories
{
    public class BetRepository : GenericRepository<Bet>, IBetRepository
    {
        public BetRepository(BetAPIContext _context) : base(_context)
        {
        }

        public async Task<List<BetDTO>> GetAllWithInfoAsync()
        {
            List<BetDTO> bets = await _context.Bet.Include(p => p.User).Include(p => p.Event).Select(
                  s => new BetDTO().SetFromBet(s)
              ).ToListAsync();

            return bets;
        }

        public async Task<IPagedList<BetDTO>> GetAllPagedWithInfoAsync(int pageNumber, int pageSize = 50)
        {
            IPagedList<BetDTO> betsPaged = await PagedListExtendedExtensions.ToPagedListAsync<BetDTO>(_context.Bet.Include(p => p.User).Include(p => p.Event).Select(
                  s => new BetDTO().SetFromBet(s)
              ), pageNumber, pageSize);

            return betsPaged;
        }

        public async Task<BetDTO?> GetByIdWithInfoAsync(int id)
        {
            Bet? bet = await _context.Bet.Include(p => p.User).Include(p => p.Event).FirstOrDefaultAsync(i => i.Id == id);

            return bet != null ? new BetDTO().SetFromBet(bet) : null;
        }

        public async Task UpdateFromDTOAsync(int id, BetPutDTO bet)
        {
            Bet? existingBet = await _context.Bet.FirstOrDefaultAsync(i => i.Id == id);
            existingBet.IsCompleted = bet.IsCompleted;
            existingBet.Payout = bet.Payout;
        }
    }
}
