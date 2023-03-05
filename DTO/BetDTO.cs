using BetAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetAPI.DTO
{
    public class BetDTO
    {
        public int Id { get; set; }

        public bool IsCompleted { get; set; }

        public int Opt { get; set; }

        public decimal Stake { get; set; }

        public decimal Payout { get; set; }

        public decimal Odds { get; set; }

        public int UserId { get; set; }
        public EventDTO Event { get; set; }

        public BetDTO SetFromBet(Bet bet)
        {
            Id = bet.Id;
            IsCompleted = bet.IsCompleted;
            Opt = bet.Opt;
            Stake = bet.Stake;
            Payout = bet.Payout;
            Odds = bet.Odds;
            UserId = bet.User.Id;
            Event = new EventDTO
            {
                Id = bet.Event.Id,
                Name = bet.Event.Name,
                Opt1 = bet.Event.Opt1,
                Opt2 = bet.Event.Opt2,
                BetsAllowedFrom = bet.Event.BetsAllowedFrom,
                StartsAt = bet.Event.StartsAt,
                EndsAt = bet.Event.EndsAt,
            };
            return this;
        }
    }
}
