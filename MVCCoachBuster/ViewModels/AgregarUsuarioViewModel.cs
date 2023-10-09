using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCCoachBuster.ViewModels
{
    public class AgregarUsuarioViewModel
    {
        public SelectList ListadoRoles { get; set; }
        public UsuarioRegistroDto Usuario { get; set; } = new UsuarioRegistroDto();
    }
}
