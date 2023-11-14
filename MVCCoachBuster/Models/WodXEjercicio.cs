using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    public class WodXEjercicio
    {
        public int Id { get; set; }
        [ForeignKey("Wod")] // Clave foránea para Wod
        public int IdWod { get; set; }

        public  Wod Wod { get; set; }
        [ForeignKey("GrupoEjercicios")] // Clave foránea para GrupoEjercicios
        public int IdGrupoEjercicios { get; set; }
        public  GrupoEjercicios GrupoEjercicios { get; set;}

        public ICollection<Progreso> Progresos { get; set; }

    }
}
