using BetAPI.Data;
using BetAPI.DTO;
using BetAPI.Models;
using BetAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using BetAPI.Generics;
using PagedList;
using BetAPI.Repositories;

namespace BetAPI.Services
{
    public class BetService: IBetService
    {
        private readonly IBetRepository _repository;
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public BetService(IBetRepository repository, IEventService eventService, IUserService userService)
        {
            _repository = repository;
            _eventService = eventService;
            _userService = userService;
        }

        public async Task<List<BetDTO>> GetBetsAsync()
        {
            return await _repository.GetAllWithInfoAsync();
        }

        public async Task<IPagedList<BetDTO>> GetBetsPagedAsync(int pageNumber, int pageSize = 50)
        {
            return await _repository.GetAllPagedWithInfoAsync(pageNumber, pageSize);
        }

        public async Task<BetDTO?> GetBetAsync(int id)
        {
            return await _repository.GetByIdWithInfoAsync(id);
        }

        public async Task<int> InsertBetAsync(Bet bet)
        {
            await _repository.InsertAsync(bet);
            return await _repository.SaveAsync();
        }

        public async Task<int> UpdateBetAsync(int id, BetPutDTO bet)
        {
            await _repository.UpdateFromDTOAsync(id, bet);
            return await _repository.SaveAsync();
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

            Bet newBet = bet.ConvertToModel();

            using var transaction = _repository.StartTransaction();
            try
            {
                int balanceResult = await _userService.UpdateBalance(bet.UserId, -bet.Stake);

                if (balanceResult == 0)
                {
                    throw new BalanceNotUpdatedException("Balance not updated when placing bet");
                }
                await _repository.InsertAsync(newBet);
                await _repository.SaveAsync();
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
            using var transaction = _repository.StartTransaction();
            try
            {
                Bet? bet = await _repository.GetByIdAsync(id);
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

                await _repository.SaveAsync();
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
