using System.ComponentModel.DataAnnotations;

namespace BetAPI.Models
{
    public class Bet: BaseEntity
    {
        public int Id { get; set; }

        public bool IsCompleted { get; set; }

        public int Opt { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Stake { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Payout { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Odds { get; set; }

        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
