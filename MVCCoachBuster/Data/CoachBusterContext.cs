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

        public DbSet<Rol> Roles { get; set; } 
        public DbSet<Usuario> Usuarios { get; set; }


        public DbSet<Plan> Planes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            base.OnModelCreating(modelBuilder);
        }
    }
}
