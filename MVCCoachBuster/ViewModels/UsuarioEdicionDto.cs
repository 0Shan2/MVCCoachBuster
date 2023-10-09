using System.ComponentModel.DataAnnotations;

namespace MVCCoachBuster.ViewModels
{
    public class UsuarioEdicionDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        [MinLength(2, ErrorMessage = "El nombre del usuario debe ser mayor o igual a 2 caracteres."),
        MaxLength(25, ErrorMessage = "El nombre del usuario no debe exceder los 25 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }
        public int Telefono { get; set; }

        [Required(ErrorMessage = "El perfil del usuario es obligatorio.")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

    }
}

