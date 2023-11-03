using Microsoft.CodeAnalysis;
using MVCCoachBuster.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    public class Dia
    {
      
        public int Id { get; set; }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }
        public string NumDias { get; set; }
        public bool? IsCompleted { get; set; }
        public ICollection<Wod> Wod { get; set; }

    }
}
