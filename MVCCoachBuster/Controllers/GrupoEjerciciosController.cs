using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;
using X.PagedList;

namespace MVCCoachBuster.Controllers
{
    public class GrupoEjerciciosController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        public GrupoEjerciciosController(CoachBusterContext context, IConfiguration configuration,
            INotyfService servicioNotificacion)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = servicioNotificacion;
        }

        //---------------------------------------------------------------------------------------------------------------------
        // GET: GrupoEjercicios
        public async Task<IActionResult> Index(ListadoViewModel<GrupoEjercicios> viewModel)
        {
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
            var consulta = _context.GrupoEjercicios
                .OrderBy(m => m.Nombre)
                .AsQueryable(); //AsQueryable para poder hacer la busqueda

            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

            return View(viewModel);
        }

        //---------------------------------------------------------------------------------------------------------------------
        // GET: GrupoEjercicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GrupoEjercicios == null)
            {
                return NotFound();
            }

            var grupoEjercicios = await _context.GrupoEjercicios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grupoEjercicios == null)
            {
                return NotFound();
            }

            return View(grupoEjercicios);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: GrupoEjercicios/Create
        public IActionResult Create(int grupoEjerciciosId)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();
            return View();
        }

        // POST: GrupoEjercicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,URLVideo,Instrucciones")] GrupoEjercicios grupoEjercicios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grupoEjercicios);
                await _context.SaveChangesAsync();
               
                // Redirige al usuario a la URL de referencia almacenada en TempData
                if (TempData.ContainsKey("UrlReferencia"))
                {
                    string urlReferencia = TempData["UrlReferencia"].ToString();
                    return Redirect(urlReferencia);
                }
               
                 return RedirectToAction(nameof(Index)); 
               // return RedirectToAction("Create", "Wods", new { grupoEjerciciosId });
            }
            return View(grupoEjercicios);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: GrupoEjercicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GrupoEjercicios == null)
            {
                return NotFound();
            }

            var grupoEjercicios = await _context.GrupoEjercicios.FindAsync(id);
            if (grupoEjercicios == null)
            {
                return NotFound();
            }
            return View(grupoEjercicios);
        }

        // POST: GrupoEjercicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,URLVideo")] GrupoEjercicios grupoEjercicios)
        {
            if (id != grupoEjercicios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grupoEjercicios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoEjerciciosExists(grupoEjercicios.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(grupoEjercicios);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: GrupoEjercicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GrupoEjercicios == null)
            {
                return NotFound();
            }

            var grupoEjercicios = await _context.GrupoEjercicios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grupoEjercicios == null)
            {
                return NotFound();
            }
            return View(grupoEjercicios);
        }

        // POST: GrupoEjercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GrupoEjercicios == null)
            {
                return Problem("Entity set 'CoachBusterContext.GrupoEjercicios'  is null.");
            }
            var grupoEjercicios = await _context.GrupoEjercicios.FindAsync(id);
            if (grupoEjercicios != null)
            {
                _context.GrupoEjercicios.Remove(grupoEjercicios);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrupoEjerciciosExists(int id)
        {
          return _context.GrupoEjercicios.Any(e => e.Id == id);
        }
    }
}
