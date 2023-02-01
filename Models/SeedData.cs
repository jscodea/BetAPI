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
                if (context.Event.Any() || context.User.Any() || context.Bet.Any())
                {
                    return;
                }

                Event FirstEvent = new Event
                {
                    Name = "Cat - Dog",
                    Opt1 = 1.85m,
                    Opt2 = 1.85m,
                    BetsAllowedFrom = DateTime.Now,
                    StartsAt = DateTime.Now.AddMinutes(5),
                    EndsAt = DateTime.Now.AddMinutes(10),
                };
                User FirstUser = new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    IsActive = true,
                    Balance = 999.99m,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };


                context.Event.AddRange(
                    FirstEvent
                );
                context.User.AddRange(
                    FirstUser
                );
                context.SaveChanges();
                context.Bet.AddRange(
                    new Bet
                    {
                        User= FirstUser,
                        Event= FirstEvent,
                        IsCompleted = false,
                        Opt = 2,
                        Stake = 10.12m,
                        Payout = 0,
                        Odds = 1.85m,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
