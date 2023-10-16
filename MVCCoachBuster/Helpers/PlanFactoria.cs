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
                UsuarioId = planDto.UsuarioId,
               
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
                UsuarioId = plan.UsuarioId,
               
            };
        }

        public void ActualizarDatosPlan(PlanCreacionEdicionDto planDto, Plan planBd)
        {
            planBd.Id = planDto.Id;
            planBd.Nombre = planDto.Nombre;
            planBd.Descripcion = planDto.Descripcion;
            planBd.Precio = planDto.Precio;
            planBd.UsuarioId = planDto.UsuarioId;
         
        }

    }
}
