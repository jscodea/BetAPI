using BetAPI.Models;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace BetAPI.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            BetIds = new List<int>();
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public decimal Balance { get; set; }

        public IEnumerable<int> BetIds { get; set; }

        public UserDTO SetFromUser(User user)
        {
            Id = user.Id;
            IsActive = user.IsActive;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Balance = user.Balance;
            BetIds = user.Bets.Select(s => s.Id);
            return this;
        }
    }
}
