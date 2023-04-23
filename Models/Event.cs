using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetAPI.Models
{
    public class Event: BaseEntity
    {
        public Event()
        {
            Bets = new HashSet<Bet>();
        }

        public int Id { get; set; }

        public string ?Name { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Opt1 { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Opt2 { get; set; }

        public int? Result { get; set; }

        [DataType(DataType.Date)]
        public DateTime BetsAllowedFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartsAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndsAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<Bet> Bets { get; set; }
    }
}
