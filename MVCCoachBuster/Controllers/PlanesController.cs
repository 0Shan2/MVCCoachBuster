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
    public class PlanesController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;

        //1º)Obtenemos acceso a IConfiguration 
        public PlanesController(CoachBusterContext context, IConfiguration configuration,
            INotyfService servicioNotificacion)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = servicioNotificacion;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes
        public async Task<IActionResult> Index(ListadoViewModel<Plan> viewModel)
        {

            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
            var consulta = _context.Planes
                .OrderBy(m => m.Nombre)
                .Include(m => m.Entrenador)
                .AsQueryable(); //AsQueryable para poder hacer la busqueda


            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

            // código asíncrono
            return View(viewModel);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Planes == null)
            {
                return NotFound();
            }

            var plan = await _context.Planes
                .Include(p => p.Entrenador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Planes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Precio,Suscrito,UsuarioId")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                // 2º)Validamos si ya hay un rol con el mismo nombre
                var existeElemtnoBd = _context.Roles
                    .Any(u => u.Nombre.ToLower().Trim() == plan.Nombre.ToLower().Trim());
                if (existeElemtnoBd)
                {
                    _servicioNotificacion.Warning("El plan que esta intentado crear, ya existe.");
                    //ModelState.AddModelError("", "El plan que esta intentado crear, ya existe.");
                    return View(plan);
                }

                try
                {
                    _context.Add(plan);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"ÉXITO al crear el plan {plan.Nombre}");
                }
                catch (DbUpdateException) // Exceptción causa por eventos externos
                {
                    _servicioNotificacion.Warning("Lo sentimos, ha ocurrido un error. Intente de nuevo.");
                    //ModelState.AddModelError("", "Lo sentimos, ha ocurrido un error. Intente de nuevo.");
                    return View(plan);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", plan.UsuarioId);
            return View(plan);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Planes == null)
            {
                return NotFound();
            }

            var plan = await _context.Planes.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", plan.UsuarioId);
            return View(plan);
        }

        // POST: Planes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Suscrito,UsuarioId")] Plan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                // 2º)Validamos que no existe otra marca con el mismo nombre
                var existeElemtnoBd = _context.Roles
                    .Any(u => u.Nombre.ToLower().Trim() == plan.Nombre.ToLower().Trim()
                    && u.Id != plan.Id);
                if (existeElemtnoBd)
                {
                    _servicioNotificacion.Warning("Ya existe un rol con este nombre.");
                    //ModelState.AddModelError("", "Ya existe un rol con este nombre.");
                    return View(plan);
                }

                try
                {
                    _context.Update(plan);
                    //Si no hay errores, la clase es actualizada correctamente
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"ÉXITO al actualziar el rol {plan.Nombre}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", plan.UsuarioId);
            return View(plan);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Planes == null)
            {
                return NotFound();
            }

            var plan = await _context.Planes
                .Include(p => p.Entrenador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Planes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Planes == null)
            {
                return Problem("Entity set 'CoachBusterContext.Planes'  is null.");
            }
            var plan = await _context.Planes.FindAsync(id);
            if (plan != null)
            {
                _context.Planes.Remove(plan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
          return _context.Planes.Any(e => e.Id == id);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        //Método para sacar las lista de entrenadores
        public IActionResult ListaEntrenadores()
        {
            //Filtra los usuarios con el rol "entrenador"
            var entrenadores = _context.Usuarios.Where( u => u.Rol.Nombre == "Entrenador").ToList();

            // Crear una lista de SelectListItem a partir de los entrenadores
            var listaEntrenadores = entrenadores.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(), // Supongo que el Id es un entero
                Text = u.Nombre
            }).ToList();

            // Agregar la lista de entrenadores a ViewBag.UsuarioId
            ViewBag.UsuarioId = listaEntrenadores;

            return View();
        }

    }
}
