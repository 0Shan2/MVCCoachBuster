using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    /* Clase que une nuestras tablas Plan con Usuarios
     */
    public class Suscripcion
    {
        public int Id { get; set; }
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        [ForeignKey("Plan")]
        public int IdPlan { get; set; }
        public Plan Plan { get; set; }
    }
}
