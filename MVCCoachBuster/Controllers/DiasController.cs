﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        public DiasController(CoachBusterContext context, IConfiguration configuration,
            INotyfService servicioNotificacion)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = servicioNotificacion;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Dias
        public async Task<IActionResult> Index(int planId)
        {
            ViewData["PlanId"] = new SelectList(_context.Planes, "Id", "Id");
            // Asigna el valor de planId a ViewBag para que se use en la vista
            ViewBag.PlanId = planId;
            // Recupera la lista de días asociados a un plan específico
            var dias = _context.Dia.Where(d => d.IdPlan == planId).ToList();

            // Cargar los Wods asociados a los días
            foreach (var dia in dias)
            {
                dia.Wod = _context.Wod.Where(w => w.IdDia == dia.Id).ToList();
            }

            // Configura ViewBag.Wod con la lista de Wods
            ViewBag.Wod = dias.SelectMany(dia => dia.Wod).ToList();


            var coachBusterContext = _context.Dia.Include(d => d.Plan).Where(d => d.IdPlan == planId);
            return View(await coachBusterContext.ToListAsync());
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Dias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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

            // Carga la relación entre Día, Wod, WodXEjercicio y GrupoEjercicios
            dia = await _context.Dia
                .Include(d => d.Wod)
                    .ThenInclude(w => w.WodXEjercicio)
                        .ThenInclude(we => we.GrupoEjercicios)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dia == null)
            {
                return NotFound();
            }
   

            return View(dia);
        }



        //-------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Dias/Create
        public IActionResult Create(int planId)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            ViewData["PlanId"] = new SelectList(_context.Planes, "Id", "Id");
            // Asigna el valor de planId a ViewBag para que se use en la vista
            ViewBag.PlanId = planId;
            // Recupera la lista de días asociados a un plan específico
            var dias = _context.Dia.Where(d => d.IdPlan == planId).ToList();

            // Cargar los Wods asociados a los días
            foreach (var dia in dias)
            {
                dia.Wod = _context.Wod.Where(w => w.IdDia == dia.Id).ToList();
            }

            // Configura ViewBag.Wod con la lista de Wods
            ViewBag.Wod = dias.SelectMany(dia => dia.Wod).ToList();
            //aquiiii agregado
            ViewBag.Dias = dias;

            return View();
        }

        // POST: Dias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int planId, [Bind("IdPlan,Nombre")] Dia dia)
        {
            if (ModelState.IsValid)
            {
                // Determina cuántos días ya existen para el plan
                int numDiasExistentes = _context.Dia.Count(d => d.IdPlan == planId);

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
                        IdPlan = planId, // Asigna el PlanId que recibes como parámetro
                        Nombre = "Día " + i, // Formatea el número de día
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
            // Si hay un error de validación, puedes manejarlo aquí
            return View(dia);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Dias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            if (id == null || _context.Dia == null)
            {
                return NotFound();
            }

            var dia = await _context.Dia
                .Include(d => d.Plan)
                .Include(w => w.Wod) 
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
            var dia = await _context.Dia
                .Include(d => d.Wod)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dia != null)
            {
                _context.Wod.RemoveRange(dia.Wod);
                _context.Dia.Remove(dia);
            }

            await _context.SaveChangesAsync();
            // Redirige al usuario a la URL de referencia almacenada en TempData
            if (TempData.ContainsKey("UrlReferencia"))
            {
                string urlReferencia = TempData["UrlReferencia"].ToString();
                return Redirect(urlReferencia);
            }
            return RedirectToAction("Create", "Dias", new { planId = id });

            //return RedirectToAction(nameof(Index));
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        private bool DiaExists(int id)
        {
            return _context.Dia.Any(e => e.Id == id);
        }

    }
}
