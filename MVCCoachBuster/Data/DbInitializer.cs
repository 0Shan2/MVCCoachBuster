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

            var usuarios = new Usuario[]
            {
                 new Usuario{Nombre="usuario", Correo="usuario@prueba.com",  Contrasena = "Usuario1@", Telefono=620380297, RolId = roles[2].Id},



            };

            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();

            //Inicialización de planes

            var planes = new Plan[]
            {
               new Plan{Nombre="Fuerza", Descripcion="eeee",  Precio = 2, Suscrito = true, UsuarioId = usuarios[0].Id},
               new Plan{Nombre="Fuerza", Descripcion="eeee",  Precio = 2, Suscrito = true, UsuarioId = usuarios[0].Id},
               new Plan{Nombre="Fuerza", Descripcion="eeee",  Precio = 2, Suscrito = true, UsuarioId = usuarios[0].Id},
               new Plan{Nombre="Fuerza", Descripcion="eeee",  Precio = 2, Suscrito = true, UsuarioId = usuarios[0].Id},

            };

            context.Planes.AddRange(planes);
            context.SaveChanges();
        }
    }
}
