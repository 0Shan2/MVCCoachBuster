using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
                .Include(m => m.usuario)
                .Include(m => m.plan)
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
                planId = idPlan,
                usuarioId = idUsu
            };

            //Guardamos la suscripcion en nuestra base
            _context.Add(suscripcion);
            await _context.SaveChangesAsync();

            return View("SuscripcionExitosa");
        }

        //---------------------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        public async Task<IActionResult> ListaPlanesSuscritos(ListadoViewModel<Plan> viewModel)
        {
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
                suscripcionesPorPlan[suscripcion.planId] = suscripcion.Id;
            }

            // Recuperamos los IDs de los planes a los que está inscrito el usuario
            List<int> idsPlanesInsc = suscripciones.Select(s => s.planId).ToList();

            //Recuperamos los objetos Plan a partir de los IDs
            var planesInscritos = _context.Planes
                .Where(p => idsPlanesInsc.Contains(p.Id))
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

        public List<Suscripcion> ObtenerSuscripciones(int usuarioId)
        {
            // Aquí suponemos que tienes un DbSet<Suscripcion> en tu contexto de Entity Framework
            // y que la entidad Suscripcion tiene una propiedad PlanId para relacionar con los planes
            return _context.Suscripcion
            .Where(s => s.usuarioId == usuarioId)
                .ToList();
        }
        

        //---------------------------------------------------------------------------------------------------------------------------------------
        // GET: Suscripcion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            if (id == null || _context.Suscripcion == null)
            {
                return NotFound();
            }

            var suscrito = await _context.Suscripcion
              .FirstOrDefaultAsync(m => m.Id == id);

            if (suscrito == null)
            {
                return NotFound();
            }

            return View(suscrito);
        }

        // POST: Suscripcion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suscripciones = await _context.Suscripcion.FindAsync(id);
            _context.Suscripcion.Remove(suscripciones);
            await _context.SaveChangesAsync();
            // Redirige al usuario a la URL de referencia almacenada en TempData
            if (TempData.ContainsKey("UrlReferencia"))
            {
                string urlReferencia = TempData["UrlReferencia"].ToString();
                return Redirect(urlReferencia);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
