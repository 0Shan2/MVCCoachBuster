using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Numerics;
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
    public class WodsController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        public WodsController(CoachBusterContext context, IConfiguration configuration,
            INotyfService servicioNotificacion)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = servicioNotificacion;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Wods
        public async Task<IActionResult> Index(ListadoViewModel<Wod>viewModel)
        {
            var registrosPorPagina = _configuration.GetValue("registrosPorPagina", 5);
            var consulta = _context.Wod
                .OrderBy(x => x.Nombre)
                .Include(w => w.Dia)
                .AsNoTracking();

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

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Wods/Details/5
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wod == null)
            {
                return NotFound();
            }

            var wod = await _context.Wod
                .Include(w => w.Dia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wod == null)
            {
                return NotFound();
            }

            return View(wod);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Wods/Create
    
        public IActionResult Create(int diaId, int planId)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();
            // Crear un nuevo Wod con el DiaId
            var wod = new Wod { DiaId = diaId };
            //Para la selección de los ejercicios
            ViewData["GrupoEjercicios"] = new MultiSelectList(_context.Set<GrupoEjercicios>(), "Id", "Nombre");
            ViewData["DiaId"] = diaId; // Añade esta línea para que DiaId esté disponible en la vista
            return View();
        }


        // POST: Wods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Wods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int diaId, [Bind("Nombre,DiaId")] Wod wod, List<int> selectedGrupoEjercicios)
        {
            if (ModelState.IsValid)
            {
                // Asigna el valor del día obtenido desde el enlace al objeto Wod
                wod.DiaId = diaId;
                // Aquí puedes realizar validaciones y guardar el nuevo Wod en la base de datos
                _context.Add(wod);
                await _context.SaveChangesAsync();
                // Obtén el nombre del Wod recién creado
                string nombreWod = wod.Nombre;

                return RedirectToAction("Create", "Dias", new { diaId = wod.DiaId });
            }

            ViewData["DiaId"] = new SelectList(_context.Set<Dia>(), "Id", "Id", wod.DiaId);
            ViewData["GrupoEjercicios"] = _context.Set<GrupoEjercicios>().ToList();
            return View(wod);
        }


        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Wods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wod == null)
            {
                return NotFound();
            }

            var wod = await _context.Wod.FindAsync(id);
            if (wod == null)
            {
                return NotFound();
            }
            ViewData["DiaId"] = new SelectList(_context.Set<Dia>(), "Id", "Id", wod.DiaId);
            return View(wod);
        }

        // POST: Wods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,DiaId")] Wod wod)
        {
            if (id != wod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WodExists(wod.Id))
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
            ViewData["DiaId"] = new SelectList(_context.Set<Dia>(), "Id", "Id", wod.DiaId);
            return View(wod);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Wods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wod == null)
            {
                return NotFound();
            }

            var wod = await _context.Wod
                .Include(w => w.Dia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wod == null)
            {
                return NotFound();
            }

            return View(wod);
        }

        // POST: Wods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wod == null)
            {
                return Problem("Entity set 'CoachBusterContext.Wod'  is null.");
            }
            var wod = await _context.Wod.FindAsync(id);
            if (wod != null)
            {
                _context.Wod.Remove(wod);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WodExists(int id)
        {
          return _context.Wod.Any(e => e.Id == id);
        }
    }
}
