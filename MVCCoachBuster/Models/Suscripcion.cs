namespace MVCCoachBuster.Models
{
    /* Clase que une nuestras tablas Plan con Usuarios
     */
    public class Suscripcion
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdPlan { get; set; }
    }
}
