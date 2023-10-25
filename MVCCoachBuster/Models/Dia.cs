using Microsoft.CodeAnalysis;

namespace MVCCoachBuster.Models
{
    public class Dia
    {
        public int Id { get; set; }

        public int PlanId { get; set; }

        public Plan Plan { get; set; }

        public int WodId { get; set; }
        public Wod Wod { get; set; }

    }
}
