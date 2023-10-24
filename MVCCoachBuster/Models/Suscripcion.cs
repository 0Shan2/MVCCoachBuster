namespace MVCCoachBuster.Models
{
    /* Clase que une nuestras tablas Plan con Usuarios
     */
    public class Suscripcion
    {
        public int Id { get; set; }
        public int usuarioId { get; set; }
        public Usuario usuario { get; set; }
        public int planId { get; set; }
        public Plan plan { get; set; }
    }
}
