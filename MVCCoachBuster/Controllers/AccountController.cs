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
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.AspNetCore.Authorization;

namespace MVCCoachBuster.Controllers
{
    public class AccountController : Controller
    {
        private readonly CoachBusterContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly INotyfService _servicioNotificacion;
        private readonly ILogger<AccountController> _logger;

        //1º)Obtenemos acceso a IConfiguration 
        public AccountController(CoachBusterContext context, IPasswordHasher<Usuario> passwordHasher,
            INotyfService servicioNotificacion, ILogger<AccountController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _servicioNotificacion = servicioNotificacion;
            _logger = logger;
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
                       new Claim(ClaimTypes.NameIdentifier, usuarioBd.Id.ToString()),
                       new Claim(ClaimTypes.Name, usuarioBd.Nombre),
					   new Claim(ClaimTypes.Email, usuarioBd.Correo),
                       new Claim(ClaimTypes.Role,usuarioBd.Rol.Nombre)
					};
                    if (!string.IsNullOrEmpty(usuarioBd.Foto))
                    {
                        claims.Add(new Claim("Foto", usuarioBd.Foto));
                    }

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

                    _logger.LogWarning("El usuario " + usuarioBd.Nombre + " ha accedido al sistema" +DateTime.Now.ToLongDateString());

					//return LocalRedirect(viewModel.ReturnUrl);
                    return RedirectToAction("Index", "Home");
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
            _logger.LogWarning("El usuario ha cerrado sesión " + DateTime.Now.ToLongDateString());
            return RedirectToAction("Index", "Home");
        }


        public ViewResult AccesoDenegado()
        {
            return View();
        }


        //------------------------------------------------------------------------------------------------------------------------------------
        // Registrar a un usuario con los campos: Correo y Contraseña
        public IActionResult Registro()
        {
            var viewModel = new RegistroUsuarioViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                // En caso de que exista un usuario con esa cuenta, mandamos error
                var existeUsuarioBd = _context.Usuarios
                    .Any(u => u.Correo.ToLower().Trim() == model.Correo.ToLower().Trim());
                if (existeUsuarioBd)
                {
                    ModelState.AddModelError("Usuario.Correo", $"Ya existe una cuenta con el correo {model.Correo}");
                    _servicioNotificacion.Warning($"Ya existe una cuenta con el correo {model.Correo}");
                    return View(model);
                }

                try
                {
                    //Para encriptar la contraseña
                    // Hashear la contraseña antes de guardarla
                    

                    // Crear un nuevo usuario en la base de datos
                    var nuevoUsuario = new Usuario
                    {
                        Nombre = model.Nombre,
                        Correo = model.Correo,
                        RolId = 3
                    };
                    nuevoUsuario.Contrasena = _passwordHasher.HashPassword(nuevoUsuario, model.Contrasena);

                    _context.Usuarios.Add(nuevoUsuario);
                    await _context.SaveChangesAsync();

                    _servicioNotificacion.Success("Registro exitoso. Ahora puedes iniciar sesión.");

                } catch (DbUpdateException ) 
                {
                    _servicioNotificacion.Warning("Lo sentimos, ha ocurrido un error. Intente nuevamente.");
                    return View(model);
                }

                return RedirectToAction("Index", "Home");  // Redirigir a la página de inicio o a donde desees
            }

            // Si el modelo no es válido, vuelve a mostrar el formulario de registro con mensajes de error
            return View("Registro", model);
        }

    }
}
