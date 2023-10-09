using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;
using X.PagedList;

namespace MVCCoachBuster.Controllers
{
    public class RolesController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;

        //1º)Obtenemos acceso a IConfiguration 
        public RolesController(CoachBusterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        // GET: Roles
        //1º) Pasamos el modelo Listado
        public async Task<IActionResult> Index(ListadoViewModel<Rol> viewModel)
        {
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
            var consulta = _context.Roles.AsQueryable(); //AsQueryable para poder hacer la busqueda

            //2º) Para buscar un rol
            if (String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                consulta = consulta.Where(u=>u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina); 
            
            // código asíncrono
              return View(viewModel);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //1º) No nos hace falta el ID, SQL server se encargará de añadirlo
        public async Task<IActionResult> Create([Bind("Nombre")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                // 2º)Validamos si ya hay un rol con el mismo nombre
                var existeElemtnoBd = _context.Roles
                    .Any(u => u.Nombre.ToLower().Trim() == rol.Nombre.ToLower().Trim());
                if (existeElemtnoBd)
                {
                    ModelState.AddModelError("", "El rol que esta intentado crear, ya existe.");
                    return View(rol);
                }

                try
                {
                    _context.Add(rol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException) // Exceptción causa por eventos externos
                {
                    ModelState.AddModelError("", "Lo sentimos, ha ocurrido un error. Intente de nuevo.");
                    return View(rol);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return View(rol);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Rol rol)
        {
            if (id != rol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // 2º)Validamos que no existe otra marca con el mismo nombre
                var existeElemtnoBd = _context.Roles
                    .Any(u => u.Nombre.ToLower().Trim() == rol.Nombre.ToLower().Trim()
                    && u.Id != rol.Id);
                if (existeElemtnoBd)
                {
                    ModelState.AddModelError("", "Ya existe un rol con este nombre.");
                    return View(rol);
                }

                try
                {
                    _context.Update(rol);
                    //Si no hay errores, la clase es actualizada correctamente
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolExists(rol.Id))
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
            return View(rol);
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'CoachBusterContext.Rol'  is null.");
            }
            var rol = await _context.Roles.FindAsync(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
          return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
