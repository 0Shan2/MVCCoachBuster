using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MVCCoachBuster.Models
{
    public class Rol
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es requerido.")]
        [Display(Name ="Rol")]
        [MaxLength(25, ErrorMessage = "El rol no puede ser más mayor a 25 caracteres.")]
        public string Nombre { get; set;}
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
