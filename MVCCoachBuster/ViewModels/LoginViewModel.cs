using System.ComponentModel.DataAnnotations;

namespace MVCCoachBuster.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "La cuenta del usuario es requerida.")]
        [Display(Name = "Cuenta")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña del usuario es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordarme?")]
        public bool Recordarme { get; set; }

        
        public string ReturnUrl { get; set; }
    }
}
