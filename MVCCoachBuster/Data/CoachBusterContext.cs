using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Models;

namespace MVCCoachBuster.Data
{
    public class CoachBusterContext : DbContext
    {
        public CoachBusterContext (DbContextOptions<CoachBusterContext> options)
            : base(options)
        {
        }

        public DbSet<MVCCoachBuster.Models.Rol> Rol { get; set; } = default!;
    }
}
