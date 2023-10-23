using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Helpers;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;
using X.PagedList;

namespace MVCCoachBuster.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;

        public SuscripcionesController(CoachBusterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index(ListadoViewModel<Suscripcion> viewModel)
        {
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
            var consulta = _context.Suscripcion
                .OrderBy(m => m.Id)
                .AsQueryable(); //AsQueryable para poder hacer la busqueda

            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

            // código asíncrono
            return View(viewModel);
        }

        private bool SuscripcionExists(int id)
        {
          return _context.Suscripcion.Any(e => e.Id == id);
        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Suscribirse(int idPlan)
        {
            //Obtenemos el id del usuario
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Registro", "Account");
            }
            int idUsu = int.Parse(userId);
            //Creamos la entrada en la tabla de suscripciones
            var suscripcion = new Suscripcion
            {
                IdPlan = idPlan,
                IdUsuario = idUsu
            };

            //Guardamos la suscripcion en nuestra base
            _context.Add(suscripcion);
            await _context.SaveChangesAsync();

            return View("SuscripcionExitosa");
        }

        //---------------------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        public async Task<IActionResult> ListaPlanesSuscritos()
        {

            //Obtenemos el id del usuario
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int idUsu = int.Parse(userId);

            //Recuperamos los IDs de los planes a los que esta inscrito el usuario
            List<int> idsPlanesInsc = _context.Suscripcion
                .Where(s => s.IdUsuario == idUsu)
                .Select(s => s.IdPlan)
                .ToList();

            //Recuperamos los objetos Plan a partir de los IDs
            List<Plan> planesInscritos = _context.Planes
                .Where(p => idsPlanesInsc.Contains(p.Id)).ToList();

            //Creamos el ViewModel
            var viewModel = new ListaPlanesSuscritosViewModel
            {
                PlanesInscritos = planesInscritos
            };

            return View(viewModel);
        }


    }
}
