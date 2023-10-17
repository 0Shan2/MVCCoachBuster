﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.Models;
using MVCCoachBuster.ViewModels;

namespace MVCCoachBuster.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IConfiguration _configuration;

        public SuscripcionesController(CoachBusterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index(ListadoViewModel<Suscripcion> viewModel)
        {

            return View(viewModel);
        }

        private bool SuscripcionExists(int id)
        {
          return _context.Suscripcion.Any(e => e.Id == id);
        }

        //--------------------------------------------------------------------------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Suscribirse(int idPlan)
        {
            //Obtenemos el id del usuario
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Registro", "Account");
            }
            int idUsu = int.Parse(userId);
            //Creamos la entrada en la tabla de suscripciones
            var suscripcion = new Suscripcion
            {
                IdPlan = idPlan,
                IdUsuario = idUsu
            };

            //Guardamos la suscripcion en nuestra base
            _context.Add(suscripcion);
            await _context.SaveChangesAsync();

            return View(suscripcion);
        }


    }
}