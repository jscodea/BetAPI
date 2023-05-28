using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetAPI.Models
{
    public abstract class AbstractEntity
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.Date)]
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
