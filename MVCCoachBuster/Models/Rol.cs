using System.Collections.ObjectModel;

namespace MVCCoachBuster.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }

    }
}
