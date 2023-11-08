using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCoachBuster.Models
{
    public class Plan
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "El nombre del entrenamiento es requerido.")]
        [MinLength(2, ErrorMessage = "El nombre del entrenamiento debe ser mayor o igual a 2 caracteres."),
        MaxLength(50, ErrorMessage = "El nombre del entrenamiento no debe exceder los 50 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(200, MinimumLength = 5,
                 ErrorMessage = "La descripción del producto debe contener entre 5 y 200 caracteres.")]
        public string Descripcion { get; set; }
        //[Required(ErrorMessage = "El costo es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Display(Name ="Entrenador")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario UsuEntrenador { get; set; }
        public string Foto { get; set; }

        public  ICollection<Dia> Dia { get; set; }
        

    }
}
