using Microsoft.CodeAnalysis;

namespace MVCCoachBuster.Models
{
    public class Dia
    {
        public int Id { get; set; }

        public int PlanId { get; set; }

        public Plan Plan { get; set; }

        public List<Wod> Wods { get; set; } //Para la navegación


    }
}
