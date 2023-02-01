using BetAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BetAPI.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            Bets = new HashSet<Bet>();
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
