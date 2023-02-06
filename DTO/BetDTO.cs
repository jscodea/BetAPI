using BetAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetAPI.DTO
{
    public class BetDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        public bool IsCompleted { get; set; }

        public int Opt { get; set; }

        public decimal Stake { get; set; }

        public decimal Payout { get; set; }

        public decimal Odds { get; set; }

        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
