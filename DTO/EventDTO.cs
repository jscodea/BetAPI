using BetAPI.Models;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace BetAPI.DTO
{
    public class EventDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public decimal Opt1 { get; set; }
        public decimal Opt2 { get; set; }

        public int? Result { get; set; }

        public DateTime BetsAllowedFrom { get; set; }

        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }

        public EventDTO SetFromEvent(Event ev)
        {
            Id = ev.Id;
            Name = ev.Name;
            Opt1 = ev.Opt1;
            Opt2 = ev.Opt2;
            Result = ev.Result;
            BetsAllowedFrom = ev.BetsAllowedFrom;
            StartsAt = ev.StartsAt;
            EndsAt = ev.EndsAt;

            return this;
        }
    }
}
