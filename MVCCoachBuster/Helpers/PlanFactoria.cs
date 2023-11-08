using MVCCoachBuster.Migrations;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;
namespace MVCCoachBuster.Helpers
{



    public class PlanFactoria
    {
        public PlanFactoria()
        {

        }

        public Plan CrearPlan(PlanCreacionEdicionDto planDto)
        {
            return new Plan
            {
                Id = planDto.Id,
                Nombre = planDto.Nombre,
                Descripcion = planDto.Descripcion,
                Precio = planDto.Precio,
                IdUsuario = planDto.IdUsuario,
               
            };
        }

        public PlanCreacionEdicionDto CrearPlan(Plan plan)
        {
            return new PlanCreacionEdicionDto
            {
                Id = plan.Id,
                Nombre = plan.Nombre,
                Descripcion = plan.Descripcion,
                Precio = plan.Precio,
                IdUsuario = plan.IdUsuario,
               
            };
        }

        public void ActualizarDatosPlan(PlanCreacionEdicionDto planDto, Plan planBd)
        {
            planBd.Id = planDto.Id;
            planBd.Nombre = planDto.Nombre;
            planBd.Descripcion = planDto.Descripcion;
            planBd.Precio = planDto.Precio;
            planBd.IdUsuario = planDto.IdUsuario;
         
        }

    }
}
