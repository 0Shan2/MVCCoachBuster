
using System.ComponentModel.DataAnnotations;

namespace MVCCoachBuster.ViewModels
{
    public class UsuarioRegistroDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        [MinLength(2, ErrorMessage = "El nombre del usuario debe ser mayor o igual a 2 caracteres."),
        MaxLength(30, ErrorMessage = "El nombre del usuario no debe exceder los 25 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

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

        public int Telefono { get; set; }

        [Required(ErrorMessage = "El perfil del usuario es obligatorio.")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

        //Añadir imagen
        public string Foto { get; set; }
    }
}
