using MVCCoachBuster.Models;
/*
 Clase en donde tenemos la inicialización de datos que nos aparecan ya una vez ejecutador el programa.
 1) Vamos a crear los roles por defecto.
 */

namespace MVCCoachBuster.Data
{
    public class DbInitializer
    {
        public static void Initialize(CoachBusterContext context)
        {
            if (context.Roles.Any())
            {
                return; // La BD ha sido inicializada con información
            }

            var roles = new Rol[]
            {
                new Rol{Nombre = "Administrador"},
                new Rol{Nombre = "Entrenador"},
                new Rol{Nombre = "Usuario"}
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }
}
