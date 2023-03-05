using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetAPI.Models
{
    public class User: BaseEntity
    {
        public User()
        {
            Bets = new HashSet<Bet>();
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Balance { get; set; }

        [JsonIgnore]
        public virtual ICollection<Bet> Bets { get; set; }
    }
}
