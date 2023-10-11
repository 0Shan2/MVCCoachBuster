using System.ComponentModel.DataAnnotations;

namespace MVCCoachBuster.ViewModels
{
    public class RegistroUsuarioViewModel
    {

        public string Nombre { get; set; }

        [Required(ErrorMessage = "La cuenta del usuario es requerida.")]
        [Display(Name = "Cuenta")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña del usuario es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string contrasena { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(5, ErrorMessage = "La contraseña debe ser mayor o igual a 5 caracteres."),
        MaxLength(20, ErrorMessage = "La contraseña no debe exceder los 20 caracteres.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
        [Required(ErrorMessage = "La confirmación de la contraseña es requerida")]
        [MinLength(5, ErrorMessage = "La confirmación de la contraseña debe ser mayor o igual a 5 caracteres."),
        MaxLength(20, ErrorMessage = "La confirmación de la contraseña no debe exceder los 20 caracteres.")]
        [DataType(DataType.Password)]
        [Compare("Contrasena", ErrorMessage = "La contraseña y su confirmación no coinciden.")]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmarContrasena { get; set; }

        [Display(Name = "Rol")]
        public int RolId { get; set; }

    }
}
