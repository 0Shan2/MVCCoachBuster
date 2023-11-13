using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    public class Progreso
    {
        public int Id { get; set; }

        [ForeignKey("IdSuscripcion")]
        public int IdSuscripcion { get; set; }
        public Suscripcion Suscripcion { get; set; }


        [ForeignKey("IdWodXEjercicio")]
        public int IdWodXEjercicio { get; set; }
        public WodXEjercicio WodXEjercicio { get; set; }

        public DateTime Fecha { get; set; }



    }
}
