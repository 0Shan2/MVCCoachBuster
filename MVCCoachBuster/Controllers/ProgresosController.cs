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

        //Logica progreso

        [HttpPost]
        public IActionResult MarcarWOD(int IdWodXEjercicio, int IdSuscripcion)
        {
            var progreso = _context.Progreso
         .FirstOrDefault(p => p.IdSuscripcion == IdSuscripcion && p.WodXEjercicio.Id == IdWodXEjercicio);

            // Cargar manualmente las entidades relacionadas
            _context.Entry(progreso)
                .Reference(p => p.WodXEjercicio)
                .Load();

            _context.Entry(progreso.WodXEjercicio)
                .Reference(we => we.Wod)
                .Load();

            if (progreso == null)
            {
                progreso = new Progreso
                {
                    IdSuscripcion = IdSuscripcion,
                    IdWodXEjercicio = IdWodXEjercicio,
                    Fecha = DateTime.Now,
                };

                _context.Progreso.Add(progreso);
            }

            progreso.IsCompleted = true; // Marcamos como completado

            _context.SaveChanges();


            return RedirectToAction("Details","Dias", new { diaId = progreso.WodXEjercicio.Wod.IdDia });
        }


        [HttpGet]
        public IActionResult ObtenerProgreso(int IdSuscripcion)
        {
            // Obtén el número total de WODs para este día
            int totalWODs = _context.Wod.Count();  // Ajusta según tus modelos y relaciones

            // Obtén el número de WODs completados para este día
            int wodsCompletados = _context.Progreso
                .Count(p => p.IdSuscripcion == IdSuscripcion && p.IsCompleted);

            // Calcular el progreso como una fracción
            string progreso = $"{wodsCompletados}/{totalWODs} completados";

            // Devuelve el progreso en formato JSON
            return Json(progreso);
        }

    }






}
