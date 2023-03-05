using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Models;
using BetAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using BetAPI.Generics;
using PagedList;

namespace BetAPI.Services
{
    public class BetService: IBetService
    {
        private readonly BetAPIContext _context;
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public BetService(BetAPIContext context, IEventService eventService, IUserService userService)
        {
            _context = context;
            _eventService = eventService;
            _userService = userService;
        }

        public async Task<List<BetDTO>> GetBetsAsync()
        {
            List<BetDTO> bets = await _context.Bet.Include(p => p.User).Include(p => p.Event).Select(
                  s => new BetDTO().SetFromBet(s)
              ).ToListAsync();

            return bets;
        }

        public async Task<IPagedList<BetDTO>> GetBetsPagedAsync(int pageNumber, int pageSize = 50)
        {
            IPagedList<BetDTO> betsPaged = await PagedListExtendedExtensions.ToPagedListAsync<BetDTO>(_context.Bet.Include(p => p.User).Include(p => p.Event).Select(
                  s => new BetDTO().SetFromBet(s)
              ), pageNumber, pageSize);

            return betsPaged;
        }

        public async Task<BetDTO?> GetBetAsync(int id)
        {
            Bet? bet = await _context.Bet.Include(p => p.User).Include(p => p.Event).FirstOrDefaultAsync(i => i.Id == id);

            return bet != null ? new BetDTO().SetFromBet(bet) : null;
        }

        public async Task<int> InsertBetAsync(Bet bet)
        {
            await _context.Bet.AddAsync(bet);
            return await _context.SaveChangesAsync();
        }

            public async Task<int> UpdateBetAsync(int id, BetPutDTO bet)
        {
            Bet? existingBet = await _context.Bet.FirstOrDefaultAsync(i => i.Id == id);
            if (existingBet == null)
            {
                return 0;
            }
            existingBet.IsCompleted = bet.IsCompleted;
            existingBet.Payout = bet.Payout;
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> BetPlaceAsync(BetPlaceDTO bet)
        {
            UserDTO? existingUser = await _userService.GetUserAsync(bet.UserId);
            if (existingUser == null || existingUser.IsActive == false)
            {
                throw new UserUnavailableException("User either not found or not active");
            }
            if (bet.Stake > existingUser.Balance)
            {
                throw new BalanceTooLowException("Not enough balance for this operation");
            }
            if (bet.Stake <= 0)
            {
                throw new MalformedDataException("Stake must be greater than 0");
            }
            EventDTO? existingEvent = await _eventService.GetEventAsync(bet.EventId);
            if (existingEvent == null || (existingEvent.BetsAllowedFrom < DateTime.Now && existingEvent.StartsAt > DateTime.Now))
            {
                throw new EventUnavailableException("Event not found or already started");
            }
            if (bet.Odds != GetEventOdds(bet.Opt, existingEvent.Opt1, existingEvent.Opt2))
            {
                throw new OddsChangedException("Odds have already changed");
            }
            Bet newBet = new Bet
            {
                IsCompleted = false,
                Opt = bet.Opt,
                Stake = bet.Stake,
                Payout = 0.00m,
                Odds = bet.Odds,
                UserId = bet.UserId,
                EventId = bet.EventId
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                int balanceResult = await _userService.UpdateBalance(bet.UserId, -bet.Stake);

                if (balanceResult == 0)
                {
                    throw new BalanceNotUpdatedException("Balance not updated when placing bet");
                }
                await _context.Bet.AddAsync(newBet);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> BetSettleAsync(int id, decimal payout)
        {
            if (payout < 0)
            {
                return false;
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Bet? bet = await _context.Bet.Include(p => p.User).Include(p => p.Event).FirstOrDefaultAsync(i => i.Id == id);
                if (bet == null || bet.IsCompleted)
                {
                    throw new BetDoesNotExistException("Non completed bet does not exists for settling");
                }
                int balanceResult = await _userService.UpdateBalance(bet.UserId, payout);

                if(balanceResult == 0)
                {
                    throw new BalanceNotUpdatedException("Balance not updated when settling bet");
                }

                bet.IsCompleted = true;
                bet.Payout = payout;

                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private decimal GetEventOdds(int Opt, decimal Opt1, decimal Opt2)
        {
            if (Opt == 1)
            {
                return Opt1;
            }
            if (Opt == 2)
            {
                return Opt2;
            }
            return 1;
        }
    }
}
