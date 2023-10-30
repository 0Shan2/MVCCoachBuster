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
    public class WodXEjerciciosController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        public WodXEjerciciosController(CoachBusterContext context, IConfiguration configuration,
            INotyfService servicioNotificacion)
        {
            _context = context;
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = servicioNotificacion;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: WodXEjercicios
        public async Task<IActionResult> Index()
        {
            ListadoViewModel<WodXEjercicio> viewModel = new ListadoViewModel<WodXEjercicio>();
            var registrosPorPagina = _configuration.GetValue("registrosPorPagina", 5);
            var consulta = _context.WodXEjercicio
                .Include(w => w.GrupoEjercicios)
                .Include(w => w.Wod)
                .AsNoTracking();

            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                consulta = consulta.Where(u => u.Wod.Nombre.Contains(viewModel.TerminoBusqueda) ||
                                                (u.Wod.DiaId != null && u.Wod.DiaId.ToString().Contains(viewModel.TerminoBusqueda)));
            }
            else
            {
                // No se aplica ningún filtro y se muestran todos los registros.
            }


            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

            
            return View(viewModel);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: WodXEjercicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WodXEjercicio == null)
            {
                return NotFound();
            }

            var wodXEjercicio = await _context.WodXEjercicio
                .Include(w => w.GrupoEjercicios)
                .Include(w => w.Wod)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wodXEjercicio == null)
            {
                return NotFound();
            }

            return View(wodXEjercicio);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: WodXEjercicios/Create
        public IActionResult Create()
        {
            ViewData["GrupoEjerciciosId"] = new SelectList(_context.GrupoEjercicios, "Id", "Id");
            ViewData["WodId"] = new SelectList(_context.Wod, "Id", "Id");
            return View();
        }

        // POST: WodXEjercicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WodId,GrupoEjerciciosId")] WodXEjercicio wodXEjercicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wodXEjercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupoEjerciciosId"] = new SelectList(_context.GrupoEjercicios, "Id", "Id", wodXEjercicio.GrupoEjerciciosId);
            ViewData["WodId"] = new SelectList(_context.Wod, "Id", "Id", wodXEjercicio.WodId);
            return View(wodXEjercicio);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: WodXEjercicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WodXEjercicio == null)
            {
                return NotFound();
            }

            var wodXEjercicio = await _context.WodXEjercicio.FindAsync(id);
            if (wodXEjercicio == null)
            {
                return NotFound();
            }
            ViewData["GrupoEjerciciosId"] = new SelectList(_context.GrupoEjercicios, "Id", "Id", wodXEjercicio.GrupoEjerciciosId);
            ViewData["WodId"] = new SelectList(_context.Wod, "Id", "Id", wodXEjercicio.WodId);
            return View(wodXEjercicio);
        }

        // POST: WodXEjercicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WodId,GrupoEjerciciosId")] WodXEjercicio wodXEjercicio)
        {
            if (id != wodXEjercicio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wodXEjercicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WodXEjercicioExists(wodXEjercicio.Id))
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
            ViewData["GrupoEjerciciosId"] = new SelectList(_context.GrupoEjercicios, "Id", "Id", wodXEjercicio.GrupoEjerciciosId);
            ViewData["WodId"] = new SelectList(_context.Wod, "Id", "Id", wodXEjercicio.WodId);
            return View(wodXEjercicio);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: WodXEjercicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WodXEjercicio == null)
            {
                return NotFound();
            }

            var wodXEjercicio = await _context.WodXEjercicio
                .Include(w => w.GrupoEjercicios)
                .Include(w => w.Wod)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wodXEjercicio == null)
            {
                return NotFound();
            }

            return View(wodXEjercicio);
        }

        // POST: WodXEjercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WodXEjercicio == null)
            {
                return Problem("Entity set 'CoachBusterContext.WodXEjercicio'  is null.");
            }
            var wodXEjercicio = await _context.WodXEjercicio.FindAsync(id);
            if (wodXEjercicio != null)
            {
                _context.WodXEjercicio.Remove(wodXEjercicio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WodXEjercicioExists(int id)
        {
          return _context.WodXEjercicio.Any(e => e.Id == id);
        }
    }
}
