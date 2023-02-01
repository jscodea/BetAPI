using BetAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BetAPI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BetAPIContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BetAPIContext>>()))
            {
                if (context.Event.Any())
                {
                    return;   // DB has been seeded
                }
                context.Event.AddRange(
                    new Event
                    {
                        Name = "Cat - Dog",
                        Opt1 = 1.85m,
                        Opt2 = 1.85m,
                        BetsAllowedFrom = DateTime.Now,
                        StartsAt= DateTime.Now.AddMinutes(5),
                        EndsAt = DateTime.Now.AddMinutes(10),
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
