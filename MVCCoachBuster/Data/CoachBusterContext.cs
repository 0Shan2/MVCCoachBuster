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

        public DbSet<MVCCoachBuster.Models.Suscripcion> Suscripcion { get; set; }
        public DbSet<GrupoEjercicios> GrupoEjercicios { get; set; }
        public DbSet<Wod> Wod { get; set; }
        public DbSet<MVCCoachBuster.Models.WodXEjercicio> WodXEjercicio { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Plan>().ToTable("Planes");
            modelBuilder.Entity<Suscripcion>().ToTable("Suscripcion");
            modelBuilder.Entity<GrupoEjercicios>().ToTable("GrupoEjercicios");
            base.OnModelCreating(modelBuilder);
        }
        

        
    }
}
