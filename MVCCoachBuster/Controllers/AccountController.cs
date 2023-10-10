using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCoachBuster.Data;
using MVCCoachBuster.ViewModels;
using System.Security.Claims;
using MVCCoachBuster.Models;

namespace MVCCoachBuster.Controllers
{
    public class AccountController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly INotyfService _servicioNotificacion;

        //1º)Obtenemos acceso a IConfiguration 
        //
        public AccountController(CoachBusterContext context, IPasswordHasher<Usuario> passwordHasher,
            INotyfService servicioNotificacion)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _servicioNotificacion = servicioNotificacion;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.ReturnUrl = returnUrl;
            return View(viewModel);
        }

       


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        public ViewResult AccesoDenegado()
        {
            return View();
        }
    }
}
