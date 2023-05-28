using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace BetAPI.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User: AbstractEntity
    {
        public User()
        {
            Bets = new HashSet<Bet>();
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public byte[] Salt { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Balance { get; set; }

        [JsonIgnore]
        public virtual ICollection<Bet> Bets { get; set; }
    }
}
