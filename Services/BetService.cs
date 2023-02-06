using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BetAPI.Services
{
    public class BetService: IBetService
    {
        private readonly BetAPIContext _context;

        public BetService(BetAPIContext context)
        {
            _context = context;
        }

        public async Task<List<BetDTO>> GetBetsAsync()
        {
            List<BetDTO> bets = await _context.Bet.Select(
                  s => new BetDTO
                  {
                      Id = s.Id,
                      IsCompleted = s.IsCompleted,
                      Opt = s.Opt,
                      Stake = s.Stake,
                      Payout = s.Payout,
                      Odds = s.Odds,
                      UserId = s.User.Id,
                      EventId = s.Event.Id
                  }
              ).ToListAsync();

            return bets;
        }

        public async Task<BetDTO?> GetBetAsync(int id)
        {
            BetDTO? bet = await _context.Bet.Select(
                  s => new BetDTO
                  {
                      Id = s.Id,
                      IsCompleted = s.IsCompleted,
                      Opt = s.Opt,
                      Stake = s.Stake,
                      Payout = s.Payout,
                      Odds = s.Odds,
                      UserId = s.User.Id,
                      EventId = s.Event.Id
                  }
              ).FirstOrDefaultAsync(i => i.Id == id);

            return bet;
        }

        public async Task<int> InsertBetAsync(Bet bet)
        {
            await _context.Bet.AddAsync(bet);
            return await _context.SaveChangesAsync();
        }
    }
}
