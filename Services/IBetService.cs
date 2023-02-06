using BetAPI.DTO;
using BetAPI.Models;

namespace BetAPI.Services
{
    public interface IBetService
    {
        Task<List<BetDTO>> GetBetsAsync();
        Task<BetDTO?> GetBetAsync(int id);
        Task<int> InsertBetAsync(Bet bet);
    }
}
