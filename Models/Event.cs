using System.ComponentModel.DataAnnotations;

namespace BetAPI.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string ?Name { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Opt1 { get; set; }

        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal Opt2 { get; set; }

        [DataType(DataType.Date)]
        public DateTime BetsAllowedFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartsAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndsAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }
    }
}
