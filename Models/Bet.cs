using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetAPI.Models
{
    public class Bet: AbstractEntity
    {
        [JsonIgnore]
        public int Id { get; set; }

        public bool IsCompleted { get; set; } = false;

        public int Opt { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Stake { get; set; } = decimal.Zero;

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Payout { get; set; } = decimal.Zero;

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Odds { get; set; } = 1;

        public int UserId { get; set; }
        public int EventId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual Event Event { get; set; }
    }
}
