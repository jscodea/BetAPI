using BetAPI.Models;
using System.ComponentModel.DataAnnotations;

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

        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
