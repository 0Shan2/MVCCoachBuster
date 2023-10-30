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
        public IActionResult Create(int planId, string nombreWod)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            ViewData["PlanId"] = new SelectList(_context.Planes, "Id", "Id");
            // Asigna el valor de planId a ViewBag para que se use en la vista
            ViewBag.PlanId = planId;
            // Recupera la lista de días asociados a un plan específico
            var dias = _context.Dia.Where(d => d.PlanId == planId).ToList();


            // Accede al valor TempData del nombre del Wod
           // TempData["NombreWod"] = nombreWod;

            // Carga los días y los Wods asociados al día
          //  var diass = _context.Dia.Include(d => d.Wod).ToList();
            ViewBag.Dias = dias;

            return View();
        }

        // POST: Dias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int planId, [Bind("PlanId,NumDias")] Dia dia)
        {

            if (ModelState.IsValid)
            {
                // Determina cuántos días ya existen para el plan
                int numDiasExistentes = _context.Dia.Count(d => d.PlanId == planId);

                // Calcula cuántas semanas completas ya se han creado
                int semanasCompletas = numDiasExistentes / 7;

                // Calcula el número de días para la próxima semana
                int diasParaProximaSemana = (semanasCompletas + 1) * 7;
                // Crea automáticamente 7 días asociados al plan
                for (int i = numDiasExistentes + 1; i <= diasParaProximaSemana; i++)
                {
                    // Crea un nuevo día y asócialo al plan con el planId
                    var nuevoDia = new Dia
                    {
                        PlanId = planId, // Asigna el PlanId que recibes como parámetro
                        NumDias = "Día " + i, // Formatea el número de día
                                              //NumDias = dia.NumDias,
                                              // Otras propiedades del día
                    };

                    _context.Add(nuevoDia);
                }
                await _context.SaveChangesAsync();

                //return RedirectToAction("Index"); // Redirige a la vista de detalles del plan u otra página deseada
                // Redirige al mismo método Create con el parámetro planId
                return RedirectToAction("Create", new { planId = planId });

            }


            // Si hay un error de validación
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
            return RedirectToAction("Create", new { planId = dia.PlanId });
        }

        private bool DiaExists(int id)
        {
            return _context.Dia.Any(e => e.Id == id);
        }
    }
}
