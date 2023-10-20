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

		public async Task<IActionResult> Index(ListadoViewModel<Plan> viewModel)
		{
           
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
			var consulta = _context.Planes
				.OrderBy(m => m.Nombre)
				.Include(m => m.Entrenador)
				.AsQueryable(); //AsQueryable para poder hacer la busqueda


            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
			{
				consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
			}

			viewModel.Total = consulta.Count();
			var numeroPagina = viewModel.Pagina ?? 1;
			viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

          

            //// Crea una lista de tareas asincrónicas para cargar las imágenes
            //var tasks = viewModel.Registros.Select(plan => Utilerias.ConvertirImagenABytes(plan.Foto, _configuration));

            //// Espera hasta que todas las tareas se completen en paralelo
            //var imagenesEnBytes = await Task.WhenAll(tasks);

            //// Asigna las imágenes en bytes a los planes
            //for (var i = 0; i < viewModel.Registros.Count; i++)
            //{
            //    viewModel.Registros[i].FotoBytes = imagenesEnBytes[i];
            //}


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