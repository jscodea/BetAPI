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
                if (!context.Event.Any())
                {
                    context.Event.AddRange(
                        new Event
                        {
                            Id = 1,
                            Name = "Cat - Dog",
                            Opt1 = 1.85m,
                            Opt2 = 1.85m,
                            BetsAllowedFrom = DateTime.Now,
                            StartsAt = DateTime.Now.AddMinutes(5),
                            EndsAt = DateTime.Now.AddMinutes(10),
                        }
                    );
                }
                if (!context.User.Any())
                {
                    context.User.AddRange(
                        new User
                        {
                            Id = 1,
                            FirstName = "John",
                            LastName = "Doe",
                            IsActive = true,
                            Balance = 999.99m,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        }
                    );
                }
                if (!context.Bet.Any())
                {
                    context.Bet.AddRange(
                        new Bet
                        {
                            Id = 1,
                            UserId = 1,
                            EventId = 1,
                            IsCompleted = false,
                            Opt = 2,
                            Stake = 10.12m,
                            Payout = 0,
                            Odds = 1.85m,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        }
                    );
                }
                context.SaveChanges();
            }
        }
    }
}
