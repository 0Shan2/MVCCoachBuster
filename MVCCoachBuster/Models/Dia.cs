using Microsoft.CodeAnalysis;
using MVCCoachBuster.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    public class Dia
    {
      
        public int Id { get; set; }
        [ForeignKey("Plan")] // Añade esta anotación para definir la relación de clave foránea
        public int IdPlan { get; set; }
        public Plan Plan { get; set; }
        public string Nombre { get; set; }
       
        public ICollection<Wod> Wod { get; set; }

    }
}
