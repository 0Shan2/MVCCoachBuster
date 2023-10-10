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

        //Esta peticion tendra un parametro return url, el cual va a contener la url a la que inteabamos acceder
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel viewModel = new LoginViewModel();
            viewModel.ReturnUrl = returnUrl;
            return View(viewModel);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            viewModel.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
				var usuarioBd = _context.Usuarios
		              .Include(u => u.Rol)
		              .FirstOrDefault(u => u.Correo.ToLower().Trim() == viewModel.Correo.ToLower().Trim());
				//si no existe el usuario enviamos una notificacion
				if (usuarioBd == null)
				{
					ModelState.AddModelError("", "Lo sentimos, el usuario no existe");
					_servicioNotificacion.Warning("Lo sentimos, el usuario no existe.");
					return View(viewModel);
				}
				//existe el usuario con el username
				var result = _passwordHasher.VerifyHashedPassword(usuarioBd, usuarioBd.Contrasena, viewModel.Password);

				if (result == PasswordVerificationResult.Success)
				{
					//La contraseña es correcta

					//Claim es un fragmento de información del usuario, en este caso
					//agregamos su correo y el rol.
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Email, usuarioBd.Correo),
						new Claim(ClaimTypes.Role,usuarioBd.Rol.Nombre)
					};

					//El ClaimIdentity es el contenedor de todos los claims del usuario.
					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					//AuthenticacionPropertyes es un diccionario de datos utilizado
					//para almacenar valores relacionados con la sesión de autenticación.
					var authProperties = new AuthenticationProperties
					{
						AllowRefresh = true,                //Permite que se refresque el tiempo de la sesión de autenticación
						IsPersistent = viewModel.Recordarme //Establece si la sesión de autenticación
															//puede ser persistente a través de múltiples peticiones
					};
					//permite crear la cookie de autenticacion
					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity),
						authProperties);

					return LocalRedirect(viewModel.ReturnUrl);
				}
                else
                {
					_servicioNotificacion.Warning("La contraseña es incorrecta");
					return View(viewModel);
				}

            }
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
