using Microsoft.CodeAnalysis;

namespace MVCCoachBuster.Models
{
    public class Wod
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public Plan Plan { get; set; }
        public int Dia { get; set; }
    }
}
