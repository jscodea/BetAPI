using BetAPI.DTO;
using BetAPI.Models;
using BetAPI.Generics;
using PagedList;

namespace BetAPI.Services
{
    public interface IBetService
    {
        Task<List<BetDTO>> GetBetsAsync();
        Task<IPagedList<BetDTO>> GetBetsPagedAsync(int pageNumber, int pageSize);
        Task<BetDTO?> GetBetAsync(int id);
        Task<int> InsertBetAsync(Bet bet);
        Task<int> UpdateBetAsync(int id, BetPutDTO bet);
        Task<bool> BetPlaceAsync(BetPlaceDTO bet);
        Task<bool> BetSettleAsync(int id, decimal payout);
    }
}
