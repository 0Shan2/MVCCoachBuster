using X.PagedList;

namespace MVCCoachBuster.ViewModels
{
    public class ListadoViewModel<T>
    {
        //Para poder buscar un objeto
        public string TerminoBusqueda { get; set; }
        public int? Pagina { get; set; }
        public IPagedList<T> Registros { get; set; }
        public int Total { get; set; } = 0;
        public string TituloCrear { get; set; }
    }
}
