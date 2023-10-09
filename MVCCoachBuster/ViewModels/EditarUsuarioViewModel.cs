using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCCoachBuster.ViewModels
{
    public class EditarUsuarioViewModel
    {
        public SelectList ListadoRoles { get; set; }
        public UsuarioEdicionDto Usuario { get; set; } = new UsuarioEdicionDto();
    }
}
