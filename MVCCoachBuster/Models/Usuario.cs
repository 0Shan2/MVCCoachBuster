using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del entrenamiento es requerido.")]
        [MinLength(2, ErrorMessage = "El nombre de usuario debe ser mayor o igual a 2 caracteres."),
        MaxLength(30, ErrorMessage = "El nombre de usuario no debe exceder los 30 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        public string Correo { get; set; }

        [Display(Name ="Contraseña")]
        public string Contrasena { get; set; }

        public int Telefono {  get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        //Añadir imagen
        public string Foto { get; set; }

        [NotMapped]
        public byte[] FotoBytes { get; set; }
        


    }
}
