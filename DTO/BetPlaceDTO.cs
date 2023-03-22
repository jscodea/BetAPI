using BetAPI.Models;

namespace BetAPI.DTO
{
    public class BetPlaceDTO
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Opt { get; set; }
        public decimal Stake { get; set; }
        public decimal Odds { get; set; }

        public Bet ConvertToModel()
        {
            return new Bet
            {
                IsCompleted = false,
                Opt = Opt,
                Stake = Stake,
                Payout = 0.00m,
                Odds = Odds,
                UserId = UserId,
                EventId = EventId
            };
        }
    }
}
