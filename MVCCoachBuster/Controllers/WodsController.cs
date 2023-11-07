using System;
using System.Collections.Generic;
using System.Configuration;
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
            ViewData["PlanId"] = new SelectList(_context.Planes, "Id", "Id");
            ViewBag.PlanId = planId; // Asigna el valor de planId a ViewBag para que se use en la vista

            ViewData["DiaId"] = new SelectList(_context.Set<Dia>(), "Id", "Id");
            ViewBag.DiaId = diaId; //Asignamos diaId a ViewBag para que se use en la vista

            //Para la selección de los ejercicos
            var grupoEjerciciosList = _context.Set<GrupoEjercicios>().ToList();
            ViewBag.GrupoEjercicios = grupoEjerciciosList;
            return View();
        }

        // POST: Wods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int diaId,int planId, [Bind("Id,Nombre,DiaId")] Wod wod, List<int> selectedGrupoEjercicios)
        {
            if (ModelState.IsValid)
            {
                wod.DiaId = diaId; //Asignamos el valor de diaId al nuevo Wod
                _context.Add(wod);
              
                await _context.SaveChangesAsync();

                int wodId = wod.Id; //Obtenemos la Id de nuevo Wod

                //Recogemos un listado de GrupoEjercicios
                if(selectedGrupoEjercicios != null)
                {
                    foreach(int grupoEjercicioId in selectedGrupoEjercicios)
                    {
                        var wodxEjercicio = new WodXEjercicio
                        {
                            WodId = wod.Id,

                            GrupoEjerciciosId = grupoEjercicioId,
                        };
                        _context.Add(wodxEjercicio);
                    }
                    await _context.SaveChangesAsync();

                }

                return RedirectToAction("Create", "Dias", new { planId = planId, diaId = diaId,  wodId = wodId });
            }
            ViewData["GrupoEjercicios"] = _context.Set<GrupoEjercicios>().ToList();
            return View(wod);
        }
        
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Wods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            if (id == null || _context.Wod == null)
            {
                return NotFound();
            }

            var wod = await _context.Wod
                .Include(w => w.WodXEjercicio) // Cargamos la lista de WodXEjercicio
                .FirstOrDefaultAsync(w => w.Id == id);
            if (wod == null)
            {
                return NotFound();
            }
            ViewData["DiaId"] = new SelectList(_context.Set<Dia>(), "Id", "Id", wod.DiaId);
            // Cargar la lista de grupos de ejercicios y asignarla a ViewBag
            ViewBag.GrupoEjercicios = await _context.GrupoEjercicios.ToListAsync();

            return View(wod);
        }

        // POST: Wods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,DiaId")] Wod wod, List<int> selectedGrupoEjercicios)
        {
            if (id != wod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cargamos el Wod existente con sus relaciones
                    var existingWod = await _context.Wod
                        .Include(w => w.WodXEjercicio)
                        .FirstOrDefaultAsync(w => w.Id == id);

                    if (existingWod == null)
                    {
                        return NotFound();
                    }

                    // Actualizamos las propiedades del Wod
                    existingWod.Nombre = wod.Nombre;
                    existingWod.DiaId = wod.DiaId;

                    // Eliminamos grupos de ejercicios deseleccionados
                    var gruposAEliminar = existingWod.WodXEjercicio
                        .Where(x => !selectedGrupoEjercicios.Contains(x.GrupoEjerciciosId))
                        .ToList();
                    _context.RemoveRange(gruposAEliminar);

                    // Agregamos grupos de ejercicios seleccionados que no estén en la lista existente
                    var gruposAAgregar = selectedGrupoEjercicios
                        .Where(x => !existingWod.WodXEjercicio.Any(e => e.GrupoEjerciciosId == x))
                        .Select(x => new WodXEjercicio { WodId = id, GrupoEjerciciosId = x })
                        .ToList();
                    _context.AddRange(gruposAAgregar);

                    // Guardamos los cambios en la base de datos
                    await _context.SaveChangesAsync();

                    // Redirige al usuario a la URL de referencia almacenada en TempData
                    if (TempData.ContainsKey("UrlReferencia"))
                    {
                        string urlReferencia = TempData["UrlReferencia"].ToString();
                        return Redirect(urlReferencia);
                    }

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
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

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
            // Redirige al usuario a la URL de referencia almacenada en TempData
            if (TempData.ContainsKey("UrlReferencia"))
            {
                string urlReferencia = TempData["UrlReferencia"].ToString();
                return Redirect(urlReferencia);
            }

            return RedirectToAction(nameof(Index));
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        private bool WodExists(int id)
        {
          return _context.Wod.Any(e => e.Id == id);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
      
    }
}
