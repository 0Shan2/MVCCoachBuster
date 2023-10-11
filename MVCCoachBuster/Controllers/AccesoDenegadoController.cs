using Microsoft.AspNetCore.Mvc;

namespace MVCCoachBuster.Controllers
{
    public class AccesoDenegadoController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
