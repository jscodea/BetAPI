using BetAPI.Data;
using BetAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System;

namespace BetAPI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, bool freshSeed = false)
        {
            using (var context = new BetAPIContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BetAPIContext>>()))
            {
                if (freshSeed)
                {
                    Console.WriteLine("Existing data is going to be deleted.");
                    context.Database.ExecuteSqlRaw("DELETE FROM Bet");
                    context.Database.ExecuteSqlRaw("DELETE FROM Event");
                    context.Database.ExecuteSqlRaw("DELETE FROM [User]");
                    Console.WriteLine("Existing data was deleted.");
                }

                if (context.Event.Any() || context.User.Any() || context.Bet.Any())
                {
                    Console.WriteLine("Data not seeded. Existing data used.");
                    return;
                }

                Console.WriteLine("Seeding data started.");

                Random r = new Random();

                Event? PreviousEvent = null;

                for (int i = 0; i < 101; i++)
                {
                    string firstParticipant = NextString(r, r.Next(3, 12));
                    string secondParticipant = NextString(r, r.Next(3, 12));
                    int EventStartsAfterMins = r.Next(5, 90);
                    decimal rOpt1 = (decimal)NextDouble(r, 1.01, 10.00, 2);
                    decimal rOpt2 = (decimal)NextDouble(r, 1.01, 10.00, 2);
                    Event SomeEvent = new Event
                    {
                        Name = $"{firstParticipant} - {secondParticipant}",
                        Opt1 = rOpt1,
                        Opt2 = rOpt2,
                        Result = null,
                        BetsAllowedFrom = DateTime.Now,
                        StartsAt = DateTime.Now.AddMinutes(EventStartsAfterMins),
                        EndsAt = DateTime.Now.AddMinutes(EventStartsAfterMins + r.Next(5, 15)),
                    };
                    context.Event.AddRange(
                            SomeEvent
                    );
                    if(PreviousEvent == null)
                    {
                        PreviousEvent = SomeEvent;
                    }
                    context.SaveChanges();   

                    for (int x = 0; x < 10; x++)
                    {
                        string password = NextString(r, r.Next(3, 12));
                        string username = NextString(r, r.Next(3, 12));
                        if (i == 0 && x == 0)
                        {
                            password = "test";
                            username = "test";
                        }
                        string hashedPassword = PasswordHelper.HashPasword(password, out var salt);
                        string jWTSecret = PasswordHelper.GetRandomSaltString();
                        string firstName = NextString(r, r.Next(3, 12));
                        string lastName = NextString(r, r.Next(3, 12));
                        User SomeUser = new User
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            IsActive = true,
                            Salt = salt,
                            JWTSecret = jWTSecret,
                            Username = username,
                            Password = hashedPassword,
                            Balance = (decimal)NextDouble(r, 0, 10000.00, 2),
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                        };
                    
                        context.User.AddRange(
                            SomeUser
                        );
                        context.SaveChanges();
                        int rOpt = r.Next(1, 2);
                        decimal chosenOdds = rOpt1;
                        if (rOpt == 2)
                        {
                            chosenOdds = rOpt2;
                        }
                        context.Bet.AddRange(
                            new Bet
                            {
                                User = SomeUser,
                                Event = SomeEvent,
                                IsCompleted = false,
                                Opt = rOpt,
                                Stake = (decimal)NextDouble(r, 0.1, 1000.00, 2),
                                Payout = 0,
                                Odds = chosenOdds,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                            }
                        );
                        rOpt = r.Next(1, 2);
                        chosenOdds = rOpt1;
                        if (rOpt == 2)
                        {
                            chosenOdds = rOpt2;
                        }
                        context.Bet.AddRange(
                            new Bet
                            {
                                User = SomeUser,
                                Event = PreviousEvent,
                                IsCompleted = false,
                                Opt = rOpt,
                                Stake = (decimal)NextDouble(r, 0.1, 1000.00, 2),
                                Payout = 0,
                                Odds = chosenOdds,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                            }
                        );
                        context.SaveChanges();
                    }
                    PreviousEvent = SomeEvent;
                }

                Console.WriteLine("Seeding data ended.");
            }
        }
        private static double NextDouble(Random rand, double minValue, double maxValue, int decimalPlaces)
        {
            double randNumber = rand.NextDouble() * (maxValue - minValue) + minValue;
            return Convert.ToDouble(randNumber.ToString("f" + decimalPlaces));
        }

        private static string NextString(Random rand, int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.

            // char is a single Unicode character
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26

            for (var i = 0; i < size; i++)
            {
                var @char = (char)rand.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
