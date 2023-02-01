using System.ComponentModel.DataAnnotations;

namespace BetAPI.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public decimal Opt1 { get; set; }
        public decimal Opt2 { get; set; }

        public DateTime BetsAllowedFrom { get; set; }

        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
    }
}
