using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Helpers;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;
using System.Configuration;
using System.Diagnostics;
using System.Security.Claims;
using X.PagedList;

namespace MVCCoachBuster.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly CoachBusterContext _context;
		private readonly IConfiguration _configuration;
		private readonly INotyfService _servicioNotificacion;
	
		public HomeController(ILogger<HomeController> logger, CoachBusterContext context, IConfiguration configuration,
			INotyfService servicioNotificacion)
        {
			_context = context;
			_configuration = configuration;
			_servicioNotificacion = servicioNotificacion;
			_logger = logger;
        }
        public List<int> ObtenerPlanesSuscritosIds()
        {
            // Obtenemos el ID del usuario actual
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new List<int>(); // Retorna una lista vacía si el usuario no está autenticado
            }

            int idUsu = int.Parse(userId);

            // Consulta para obtener los IDs de los planes a los que el usuario está suscrito
            var planesSuscritosIds = _context.Suscripcion
                .Where(s => s.IdUsuario == idUsu)
                .Select(s => s.IdPlan)
                .ToList();

            return planesSuscritosIds;
        }
        public List<Suscripcion> ObtenerSuscripciones(int usuarioId)
        {
            // Aquí suponemos que tienes un DbSet<Suscripcion> en tu contexto de Entity Framework
            // y que la entidad Suscripcion tiene una propiedad PlanId para relacionar con los planes
            return _context.Suscripcion
            .Where(s => s.IdUsuario == usuarioId)
                .ToList();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        public async Task<IActionResult> ListaPlanesSuscritos(ListadoViewModel<Plan> viewModel)
        {
            // Obtén los IDs de los planes a los que el usuario está suscrito
            var planesSuscritosIds = ObtenerPlanesSuscritosIds();

            // Almacena la lista de IDs en TempData para usar en otras acciones
            TempData["PlanesSuscritosIds"] = planesSuscritosIds;

            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);

            //Obtenemos el id del usuario
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int idUsu = int.Parse(userId);

            // Recuperamos las suscripciones del usuario
            var suscripciones = ObtenerSuscripciones(idUsu);

            // Creamos un Dictionary para mapear los Ids de las suscripciones a los planes
            var suscripcionesPorPlan = new Dictionary<int, int>();

            foreach (var suscripcion in suscripciones)
            {
                suscripcionesPorPlan[suscripcion.IdPlan] = suscripcion.Id;
            }

            // Recuperamos los IDs de los planes a los que está inscrito el usuario
            List<int> idsPlanesInsc = suscripciones.Select(s => s.IdPlan).ToList();

            //Recuperamos los objetos Plan a partir de los IDs
            var planesInscritos = _context.Planes
                .Where(p => idsPlanesInsc.Contains(p.Id))
                .Include(m => m.UsuEntrenador)
                .OrderBy(m => m.Nombre)
                .AsNoTracking();

            //Creamos el ViewModel
            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                planesInscritos = planesInscritos.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.Total = planesInscritos.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await planesInscritos.ToPagedListAsync(numeroPagina, registrosPorPagina);
            // Pasamos el Dictionary de suscripcionesPorPlan al ViewModel
            viewModel.SuscripcionesPorPlan = suscripcionesPorPlan;

            return View(viewModel);
        }
        public async Task<IActionResult> Index(ListadoViewModel<Plan> viewModel)
		{
            // Obtén los IDs de los planes a los que el usuario está suscrito
            var planesSuscritosIds = ObtenerPlanesSuscritosIds();

            // Resto del código para obtener la lista de planes y su visualización

            // Asigna planesSuscritosIds al modelo
            viewModel.PlanesSuscritosIds = planesSuscritosIds;
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
			var consulta = _context.Planes
				.OrderBy(m => m.Nombre)
				.Include(m => m.UsuEntrenador)
				.AsQueryable(); //AsQueryable para poder hacer la busqueda


            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
			{
				consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
			}

			viewModel.Total = consulta.Count();
			var numeroPagina = viewModel.Pagina ?? 1;
			viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

            // código asíncrono
            return View(viewModel);
		}
        public async Task<IActionResult> Planes(ListadoViewModel<Plan> viewModel)
        {
            // Obtén los IDs de los planes a los que el usuario está suscrito
            var planesSuscritosIds = ObtenerPlanesSuscritosIds();

            // Asigna planesSuscritosIds al modelo
            viewModel.PlanesSuscritosIds = planesSuscritosIds;
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
            var consulta = _context.Planes
                .OrderBy(m => m.Nombre)
                .Include(m => m.UsuEntrenador)
                .AsQueryable(); //AsQueryable para poder hacer la busqueda


            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

            // código asíncrono
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}