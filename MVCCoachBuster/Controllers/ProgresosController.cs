using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Models;

namespace MVCCoachBuster.Controllers
{
    public class ProgresosController : Controller
    {
        private readonly CoachBusterContext _context;

        public ProgresosController(CoachBusterContext context)
        {
            _context = context;
        }

        // GET: Progresos
        public async Task<IActionResult> Index()
        {
              return View(await _context.Progreso.ToListAsync());
        }




        // GET: Progresos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Progreso == null)
            {
                return NotFound();
            }

            var progreso = await _context.Progreso
                .FirstOrDefaultAsync(m => m.Id == id);
            if (progreso == null)
            {
                return NotFound();
            }

            return View(progreso);
        }

        // GET: Progresos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Progresos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdSuscripcion,IdWodXEjercicio,Fecha")] Progreso progreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(progreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(progreso);
        }

        // GET: Progresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Progreso == null)
            {
                return NotFound();
            }

            var progreso = await _context.Progreso.FindAsync(id);
            if (progreso == null)
            {
                return NotFound();
            }
            return View(progreso);
        }

        // POST: Progresos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdSuscripcion,IdWodXEjercicio,Fecha")] Progreso progreso)
        {
            if (id != progreso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(progreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgresoExists(progreso.Id))
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
            return View(progreso);
        }

        // GET: Progresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Progreso == null)
            {
                return NotFound();
            }

            var progreso = await _context.Progreso
                .FirstOrDefaultAsync(m => m.Id == id);
            if (progreso == null)
            {
                return NotFound();
            }

            return View(progreso);
        }

        // POST: Progresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Progreso == null)
            {
                return Problem("Entity set 'CoachBusterContext.Progreso'  is null.");
            }
            var progreso = await _context.Progreso.FindAsync(id);
            if (progreso != null)
            {
                _context.Progreso.Remove(progreso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgresoExists(int id)
        {
          return _context.Progreso.Any(e => e.Id == id);
        }
    }
}
