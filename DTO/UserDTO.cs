using BetAPI.Models;
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

        public List<int> BetIds { get; set; }
    }
}
