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
    public class DiasController : Controller
    {
        private readonly CoachBusterContext _context;

        public DiasController(CoachBusterContext context)
        {
            _context = context;
        }

        // GET: Dias
        public async Task<IActionResult> Index()
        {
            var coachBusterContext = _context.Dia.Include(d => d.Plan);
            return View(await coachBusterContext.ToListAsync());
        }

        // GET: Dias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dia == null)
            {
                return NotFound();
            }

            var dia = await _context.Dia
                .Include(d => d.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dia == null)
            {
                return NotFound();
            }

            return View(dia);
        }

        // GET: Dias/Create
        public IActionResult Create()
        {
        
            return View();
        }

        // POST: Dias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanId,NumDias")] Dia dia)
        {
            if (ModelState.IsValid)
            {
                var nuevoDia = new Dia
                {
                    PlanId = dia.PlanId, // Establece el PlanId desde la URL
                    NumDias = dia.NumDias,
                    // Otras propiedades del día
                };
                _context.Add(nuevoDia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanId"] = new SelectList(_context.Planes, "Id", "Id", dia.PlanId);
            return View(dia);
        }

        // GET: Dias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dia == null)
            {
                return NotFound();
            }

            var dia = await _context.Dia.FindAsync(id);
            if (dia == null)
            {
                return NotFound();
            }
            ViewData["PlanId"] = new SelectList(_context.Planes, "Id", "Id", dia.PlanId);
            return View(dia);
        }

        // POST: Dias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanId,numDias")] Dia dia)
        {
            if (id != dia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaExists(dia.Id))
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
            ViewData["PlanId"] = new SelectList(_context.Planes, "Id", "Id", dia.PlanId);
            return View(dia);
        }

        // GET: Dias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dia == null)
            {
                return NotFound();
            }

            var dia = await _context.Dia
                .Include(d => d.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dia == null)
            {
                return NotFound();
            }

            return View(dia);
        }

        // POST: Dias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dia == null)
            {
                return Problem("Entity set 'CoachBusterContext.Dia'  is null.");
            }
            var dia = await _context.Dia.FindAsync(id);
            if (dia != null)
            {
                _context.Dia.Remove(dia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiaExists(int id)
        {
          return _context.Dia.Any(e => e.Id == id);
        }
    }
}
