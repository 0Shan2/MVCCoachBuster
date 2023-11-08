using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    public class Wod
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [ForeignKey("Dia")]
        public int? IdDia { get; set; }
        public Dia Dia { get; set; }
        public bool IsCompleted { get; set; }
        public ICollection<WodXEjercicio> WodXEjercicio { get; set; }
    }
}
