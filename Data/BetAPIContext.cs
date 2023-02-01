using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using BetAPI.Models;
namespace BetAPI.Data
{
    public class BetAPIContext: DbContext
    {
        public BetAPIContext(DbContextOptions<BetAPIContext> options)
            : base(options)
        {
        }

        public DbSet<BetAPI.Models.Event> Event { get; set; } = default!;

        public DbSet<BetAPI.Models.User> User { get; set; } = default!;

        public DbSet<BetAPI.Models.Bet> Bet { get; set; } = default!;
    }
}
