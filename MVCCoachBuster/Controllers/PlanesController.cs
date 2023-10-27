using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Helpers;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;
using X.PagedList;

namespace MVCCoachBuster.Controllers
{
    [Authorize(Policy = "Gestión")]
    public class PlanesController : Controller
    {
      

        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;
        private readonly PlanFactoria _planFactoria;

        //1º)Obtenemos acceso a IConfiguration 
        public PlanesController(CoachBusterContext context, IConfiguration configuration,
            INotyfService servicioNotificacion, PlanFactoria planFactoria)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = servicioNotificacion;
            _planFactoria = planFactoria;

        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes
        [AllowAnonymous]
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
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

           // plan.FotoBytes = await Utilerias.ConvertirImagenABytes(plan.Foto, _configuration);


            return View(plan);

          
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes/Create

        public IActionResult Create()
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            AgregarEditarPlanViewModel viewModel = new AgregarEditarPlanViewModel();

            viewModel.ListadoEntrenadores = new SelectList(_context.Usuarios.Where(u => u.Rol.Id == 2).AsNoTracking(), "Id", "Nombre");
                                                                                                              
            return View("Plan", viewModel);
        }

        // POST: Planes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Precio,UsuarioId,Foto")] PlanCreacionEdicionDto plan, string siguiente)
        {
           
            AgregarEditarPlanViewModel viewModel = new AgregarEditarPlanViewModel();
            viewModel.ListadoEntrenadores = new SelectList(_context.Usuarios.Where(u => u.Rol.Id == 2).AsNoTracking(), "Id", "Nombre", plan.UsuarioId);
   

            viewModel.Plan = plan;


            if (ModelState.IsValid)
            {

                // 2º)Validamos si ya hay un plan con el mismo nombre
                var existeElemtnoBd = _context.Planes
                    .Any(u => u.Nombre.ToLower().Trim() == plan.Nombre.ToLower().Trim());
                if (existeElemtnoBd)
                {
                    ModelState.AddModelError("Plan.Nombre", "Lo sentimos, ya existe un elemento con el nombre indicado.");
                    _servicioNotificacion.Warning("El plan que esta intentado crear, ya existe.");
                    //ModelState.AddModelError("", "El plan que esta intentado crear, ya existe.");
                    return View("Plan", viewModel);
                }

                try
                {
                    var nuevoPlan = _planFactoria.CrearPlan(plan);
                    // Redirige al usuario a la vista de creación de "Dias" y pasa el ID del nuevo plan como parámetro
                    if (siguiente == "true")
                    {
                        // Redirige al usuario a la vista de creación de "Dias" y pasa el ID del nuevo plan como parámetro
                        return RedirectToAction("Create", "Dias", new { planId = nuevoPlan.Id });
                    }
                    else
                    {
                        // Redirige al usuario a la vista de "Planes" o a otra vista de tu elección
                        return RedirectToAction("Index");
                    }
                    //Para añadir la imagen
                    if (Request.Form.Files.Count > 0)
                    {
                       
                        IFormFile archivo = Request.Form.Files.FirstOrDefault();
                        nuevoPlan.Foto = await Utilerias.LeerImagen(archivo,_configuration);
                        // Agrega registros de depuración para verificar el valor de nuevoPlan.Foto
                        Console.WriteLine($"Valor de nuevoPlan.Foto después de cargar la imagen: {nuevoPlan.Foto}");

                    }
            
                    _context.Add(nuevoPlan);
                    await _context.SaveChangesAsync();
      
                    _servicioNotificacion.Success($"ÉXITO al crear el plan {plan.Nombre}");
                }
                catch (DbUpdateException) // Exceptción causa por eventos externos
                {
                    _servicioNotificacion.Warning("Lo sentimos, ha ocurrido un error. Intente de nuevo.");
                    //ModelState.AddModelError("", "Lo sentimos, ha ocurrido un error. Intente de nuevo.");
                    return View("Plan", viewModel);
                }
                // Redirige al usuario a la URL de referencia almacenada en TempData
                if (TempData.ContainsKey("UrlReferencia"))
                {
                    string urlReferencia = TempData["UrlReferencia"].ToString();
                    return Redirect(urlReferencia);
                }

                return RedirectToAction("Index"); // Si no hay URL de referencia en TempData
            }

            return View("Plan", viewModel);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            if (id == null || _context.Planes == null)
            {
                return NotFound();
            }

            var plan = await _context.Planes.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            AgregarEditarPlanViewModel viewModel = new AgregarEditarPlanViewModel();
            
            viewModel.Plan = _planFactoria.CrearPlan(plan);
            viewModel.Plan.Foto = plan.Foto;
            viewModel.ListadoEntrenadores = new SelectList(_context.Usuarios
                .Where(u => u.Rol.Id == 2)
                .AsNoTracking(), "Id", "Nombre", plan.UsuarioId);

            return View("Plan", viewModel);

        }

        // POST: Planes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,UsuarioId, Foto")] PlanCreacionEdicionDto plan)
        {

            AgregarEditarPlanViewModel viewModel = new AgregarEditarPlanViewModel();
            viewModel.ListadoEntrenadores = new SelectList(_context.Usuarios.Where(u => u.Rol.Id == 2).AsNoTracking(), "Id", "Nombre", plan.UsuarioId);
            viewModel.Plan = plan;

            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // 2º)Validamos que no existe otra marca con el mismo nombre
                var existeElemtnoBd = _context.Planes
                    .Any(u => u.Nombre.ToLower().Trim() == plan.Nombre.ToLower().Trim()
                    && u.Id != plan.Id);
                if (existeElemtnoBd)
                {
                    _servicioNotificacion.Warning("Ya existe un rol con este nombre.");
                    //ModelState.AddModelError("", "Ya existe un rol con este nombre.");
                    return View("Plan", viewModel);
                }

                try
                {
                    var planBd = await _context.Planes.FindAsync(plan.Id);
                    _planFactoria.ActualizarDatosPlan(plan, planBd);

                    //Para añadir la imagen
                    if (Request.Form.Files.Count > 0)
                    {
                        IFormFile archivo = Request.Form.Files.FirstOrDefault();
                        planBd.Foto = await Utilerias.LeerImagen(archivo, _configuration);
                    }

                    _context.Update(planBd);
                     //Si no hay errores, la clase es actualizada correctamente
                     await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"ÉXITO al actualizar el rol {plan.Nombre}");
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
                // Redirige al usuario a la URL de referencia almacenada en TempData
                if (TempData.ContainsKey("UrlReferencia"))
                {
                    string urlReferencia = TempData["UrlReferencia"].ToString();
                    return Redirect(urlReferencia);
                }

                return RedirectToAction("Index"); // Si no hay URL de referencia en TempData
            }

            return View("Plan", viewModel);
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Planes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

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
            // Redirige al usuario a la URL de referencia almacenada en TempData
            if (TempData.ContainsKey("UrlReferencia"))
            {
                string urlReferencia = TempData["UrlReferencia"].ToString();
                return Redirect(urlReferencia);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Planes.Any(e => e.Id == id);
        }

        //---------------------------------------------------------------------------------------------------------------------------------
        //Listado de planes del entrenador, que nos muestra sus creaciones de planes 
        [Authorize]
        public async Task<IActionResult> ListaPlanesCreados(ListadoViewModel<Plan> viewModel)
        {
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);

            // Cogemos el usuario logueado, en la vista nos aseguramos de que sea entrenador 
            string coachId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int idCoach = int.Parse(coachId);

            //Recuperamos la lista de coach con sus entrenamientos
            var planesCreados = _context.Planes
                .Where(p => p.UsuarioId == idCoach)
                .OrderBy(m => m.Nombre)
                .AsNoTracking();


            //2º) Para buscar un plan
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                planesCreados = planesCreados.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.Total = planesCreados.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await planesCreados.ToPagedListAsync(numeroPagina, registrosPorPagina);


            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ListadoDias(ListadoViewModel<Dia> viewModel)
        {
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);
            var consulta = _context.Dia
                .OrderBy(d => d.NumDias)
                .AsQueryable(); // AsQueryable para poder hacer la búsqueda

            // Puedes agregar filtros o búsquedas similares a las que tienes en la acción para los planes.

            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = consulta.ToPagedList(numeroPagina, registrosPorPagina);

            return View(viewModel);
        }
        public IActionResult RedirigirADias()
        {
            // Puedes redirigir a la acción "Index" en el controlador "Dias"
            return RedirectToAction("Index", "Dias");
        }



    }
}
