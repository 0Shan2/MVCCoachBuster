using Microsoft.AspNetCore.Mvc.Rendering;
using MVCCoachBuster.Models;

namespace MVCCoachBuster.ViewModels
{
    public class AgregarEditarPlanViewModel
    {
        public SelectList ListadoEntrenadores { get; set; }

        public PlanCreacionEdicionDto Plan { get; set; } = new PlanCreacionEdicionDto();
    }
}
