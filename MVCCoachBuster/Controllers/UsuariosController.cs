﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
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
    public class UsuariosController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;
        private readonly INotyfService _servicioNotificacion;
        private readonly UsuarioFactoria _usuarioFactoria;

        public UsuariosController(CoachBusterContext context, IConfiguration configuration,
            INotyfService servicioNotificacion, UsuarioFactoria usuarioFactoria)
        {
            _context = context;
            _configuration = configuration;
            _servicioNotificacion = servicioNotificacion;
            _usuarioFactoria = usuarioFactoria;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Usuarios
        public async Task<IActionResult> Index(ListadoViewModel<Usuario> viewModel)
        {
            var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);

            var consulta = _context.Usuarios
                            .Include(u => u.Rol)
                            .OrderBy(m => m.Nombre)
                            .AsNoTracking();

            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda)
                                || u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.TituloCrear = "Crear Usuario";
            viewModel.Total = consulta.Count();
            var numeroPagina = viewModel.Pagina ?? 1;

            viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

            return View(viewModel);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Usuarios/Create
        [Authorize(Policy = "Admin")]
        public IActionResult Create()
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            AgregarUsuarioViewModel viewModel = new AgregarUsuarioViewModel();
            viewModel.ListadoRoles = new SelectList(_context.Roles.AsNoTracking(), "Id", "Nombre");
            return View(viewModel);
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Correo,Contrasena,ConfirmarContrasena,Telefono,IdRol,Foto")] UsuarioRegistroDto usuario)
        {
            AgregarUsuarioViewModel viewModel = new AgregarUsuarioViewModel();
            viewModel.ListadoRoles = new SelectList(_context.Roles.AsNoTracking(), "Id", "Nombre");
            viewModel.Usuario = usuario;

            if (ModelState.IsValid)
            {
                // En caso de que exista un usuario con esa cuenta, mandamos error
                var existeUsuarioBd = _context.Usuarios
                    .Any(u => u.Correo.ToLower().Trim() == usuario.Correo.ToLower().Trim());
                if (existeUsuarioBd)
                {
                    ModelState.AddModelError("Usuario.Correo", $"Ya existe una cuenta con el correo {usuario.Correo}");
                    _servicioNotificacion.Warning($"Ya existe una cuenta con el correo {usuario.Correo}");
                    return View(viewModel);
                }
                
                try
                {
                    var usuarioAgregar = _usuarioFactoria.CrearUsuario(usuario);
                    
                    //Para añadir la imagen
                    if(Request.Form.Files.Count > 0)
                    {
                        IFormFile archivo = Request.Form.Files.FirstOrDefault();
                        usuarioAgregar.Foto = await Utilerias.LeerImagen(archivo, _configuration);
                    }
                    
                    _context.Usuarios.Add(usuarioAgregar);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"ÉXITO al crear el usuario con correo {usuario.Correo}");
                }
                catch (DbUpdateException)
                {
                    _servicioNotificacion.Warning("Lo sentimos, ha ocurrido un error. Intente nuevamente.");
                    return View(viewModel);
                }
                // Redirige al usuario a la URL de referencia almacenada en TempData
                if (TempData.ContainsKey("UrlReferencia"))
                {
                    string urlReferencia = TempData["UrlReferencia"].ToString();
                    return Redirect(urlReferencia);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            EditarUsuarioViewModel viewModel = new EditarUsuarioViewModel();
            viewModel.ListadoRoles = new SelectList(_context.Roles.AsNoTracking(), "Id", "Nombre");
            viewModel.Usuario = _usuarioFactoria.CrearUsuarioEdicion(usuario);
            viewModel.Usuario.Foto = usuario.Foto; // Supongamos que la propiedad FotoPath contiene la ruta de la imagen

            return View(viewModel);
        }
        
        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Perfil(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            EditarUsuarioViewModel viewModel = new EditarUsuarioViewModel();
            viewModel.ListadoRoles = new SelectList(_context.Roles.AsNoTracking(), "Id", "Nombre");
            viewModel.Usuario = _usuarioFactoria.CrearUsuarioEdicion(usuario);
            viewModel.Usuario.IdRol = usuario.IdRol;
            viewModel.Usuario.Foto = usuario.Foto;
            
            return View("Perfil",viewModel);
        }
        
        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Perfil(int id, [Bind("Id,Nombre,Correo,Telefono,IdRol, Foto")] UsuarioEdicionDto usuario)
        {
            EditarUsuarioViewModel viewModel = new EditarUsuarioViewModel();
            viewModel.ListadoRoles = new SelectList(_context.Roles.AsNoTracking(), "Id", "Nombre");
            viewModel.Usuario = usuario;

            if (id != usuario.Id)
            {
                return NotFound();
            }
        
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioBd = await _context.Usuarios.FindAsync(usuario.Id);

                    _usuarioFactoria.ActualizarDatosUsuario(usuario, usuarioBd);

                    //Para añadir la imagen
                    if (Request.Form.Files.Count > 0)
                    {
                        IFormFile archivo = Request.Form.Files.FirstOrDefault();
                        usuarioBd.Foto = await Utilerias.LeerImagen(archivo, _configuration);
                    }
          
                    _context.Update(usuarioBd);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"ÉXITO al actualizar el usuario cuyo correo es: {usuario.Correo}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
                return RedirectToAction("Perfil");
            }
            return View("Perfil");
        }


        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Correo,Telefono,IdRol, Foto")] UsuarioEdicionDto usuario)
        {
            EditarUsuarioViewModel viewModel = new EditarUsuarioViewModel();
            viewModel.ListadoRoles = new SelectList(_context.Roles.AsNoTracking(), "Id", "Nombre");
            viewModel.Usuario = usuario;

            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioBd = await _context.Usuarios.FindAsync(usuario.Id);
                    _usuarioFactoria.ActualizarDatosUsuario(usuario, usuarioBd);

                    //Para añadir la imagen
                    if (Request.Form.Files.Count > 0)
                    {
                        IFormFile archivo = Request.Form.Files.FirstOrDefault();
                        usuarioBd.Foto = await Utilerias.LeerImagen(archivo, _configuration);
                    }

                    _context.Update(usuarioBd);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"ÉXITO al actualizar el usuario cuyo correo es: {usuario.Correo}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
                return RedirectToAction(nameof(Index));
            }
            
            return View(viewModel);
        }

  

        //------------------------------------------------------------------------------------------------------------------------------------------------
        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["UrlReferencia"] = Request.Headers["Referer"].ToString();

            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'CoachBusterContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
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

        private bool UsuarioExists(int id)
        {
          return _context.Usuarios.Any(e => e.Id == id);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> CambiarContrasena(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            CambiarContrasenaViewModel viewModel = _usuarioFactoria.CrearCambiarContrasenaViewModel(usuario);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarContrasena(int id, [Bind("Id,Correo,Contrasena,ConfirmarContrasena")] CambiarContrasenaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioBd = await _context.Usuarios.FindAsync(viewModel.Id);
                    _usuarioFactoria.ActualizarContrasenaUsuario(viewModel, usuarioBd);
                    await _context.SaveChangesAsync();
                    _servicioNotificacion.Success($"ÉXITO al actualizar la contraseña del usuario: {usuarioBd.Correo}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(viewModel.Id))
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
            return View(viewModel);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Listado de Entrenadores
        public async Task<IActionResult> ListaEntrenadores(ListadoViewModel<Usuario> viewModel)
        {

            var entrenadores = _context.Usuarios
                .Where(m => m.IdRol == 2)
                .AsNoTracking();

            // Para buscar a un entrenador
            if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
            {
                entrenadores = entrenadores.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
            }

            viewModel.Total = entrenadores.Count();
            var numeroPagina = viewModel.Pagina ?? 1;
            viewModel.Registros = await entrenadores.ToPagedListAsync(numeroPagina, 10);

            return View(viewModel);
        }
        

    }
}
