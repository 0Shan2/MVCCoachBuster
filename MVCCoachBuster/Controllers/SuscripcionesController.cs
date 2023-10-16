using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Models;

namespace MVCCoachBuster.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly CoachBusterContext _context;

        public SuscripcionesController(CoachBusterContext context)
        {
            _context = context;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index()
        {
              return View(await _context.Suscripcion.ToListAsync());
        }

        // GET: Suscripciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Suscripcion == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // GET: Suscripciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,IdPlan")] Suscripcion suscripcion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suscripcion);
        }

        // GET: Suscripciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Suscripcion == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }
            return View(suscripcion);
        }

        // POST: Suscripciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,IdPlan")] Suscripcion suscripcion)
        {
            if (id != suscripcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscripcionExists(suscripcion.Id))
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
            return View(suscripcion);
        }

        // GET: Suscripciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Suscripcion == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // POST: Suscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Suscripcion == null)
            {
                return Problem("Entity set 'CoachBusterContext.Suscripcion'  is null.");
            }
            var suscripcion = await _context.Suscripcion.FindAsync(id);
            if (suscripcion != null)
            {
                _context.Suscripcion.Remove(suscripcion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuscripcionExists(int id)
        {
          return _context.Suscripcion.Any(e => e.Id == id);
        }

        //--------------------------------------------------------------------------------------------------------------------------
        private int ObtenerIdUsuario()
        {
            // Verifica si el usuario actual está autenticado
            if (User.Identity.IsAuthenticated)
            {
                // Obtiene el Id del usuario autenticado
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    // Convierte el Id de usuario a un entero (puedes ajustar según tu implementación)
                    if (int.TryParse(userId, out int id))
                    {
                        return id;
                    }
                }
            }

            // Si no se encuentra un usuario autenticado o no se puede obtener el Id, puedes devolver un valor predeterminado, como -1 o lanzar una excepción.
            return -1;
        }

        private bool UsuarioEstaRegistrado(int idUsuario)
        {

            // Verifica si el usuario con el ID proporcionado existe en la base de datos
            var usuarioRegistrado = _context.Usuarios.Any(u => u.Id == idUsuario);

            return usuarioRegistrado;
        }

        [HttpPost]
        public async Task<IActionResult> InscribirseAPlan(int idPlan)
        {
            // Aquí puedes obtener el IdUsuario de la sesión o de alguna otra forma, dependiendo de tu sistema
            int idUsuario = ObtenerIdUsuario(); // Debes implementar esta función

            // Verificar si el usuario ya está suscrito al plan
            var suscripcionExistente = await _context.Suscripcion
                .FirstOrDefaultAsync(s => s.IdUsuario == idUsuario && s.IdPlan == idPlan);

            if (suscripcionExistente != null)
            {
                return BadRequest("Ya estás inscrito a este plan.");
            }

            // Si el usuario no está registrado, redirige al proceso de registro
            if (!UsuarioEstaRegistrado(idUsuario)) // Debes implementar esta función
            {
                return RedirectToAction("Registro"); // Redirige al formulario de registro
            }

            // El usuario está registrado, procede a crear la suscripción
            var nuevaSuscripcion = new Suscripcion
            {
                IdUsuario = idUsuario,
                IdPlan = idPlan
            };

            _context.Suscripcion.Add(nuevaSuscripcion);
            await _context.SaveChangesAsync();

            return Ok("Te has inscrito al plan exitosamente.");
        }
    }
}
