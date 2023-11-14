using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        /*
        [HttpPost]
        public IActionResult MarcarWOD(int IdWodXEjercicio, int IdSuscripcion)
        {
            try
            {
                var progreso = _context.Progreso.SingleOrDefault(p => p.IdWodXEjercicio == IdWodXEjercicio);

                if (progreso == null || progreso.WodXEjercicio == null)
                {
                    return NotFound("No se encontró el progreso o el WodXEjercicio asociado.");
                }

                progreso.IsCompleted = true;

                // Utiliza una transacción para garantizar operaciones atómicas
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw; // Re-lanza la excepción después de hacer rollback
                    }
                }

                return RedirectToAction("Details", "Dias", new { id = IdSuscripcion });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al marcar el WOD como completado: {ex.Message}");
            }
        }
        
        [HttpGet]
        public IActionResult ObtenerProgreso(int IdSuscripcion)
        {
            try
            {
                var progreso = _context.Progreso.Count(p => p.IdSuscripcion == IdSuscripcion && p.IsCompleted);

                var totalWODs = _context.Progreso
                    .Where(p => p.IdSuscripcion == IdSuscripcion)
                    .Select(p => p.WodXEjercicio)
                    .Distinct()
                    .Count();

                var progresoActualizado = $"{progreso}/{totalWODs} completados";

                return Ok(progresoActualizado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el progreso actualizado: {ex.Message}");
            }
        }
        */


    }






}
