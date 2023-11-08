namespace MVCCoachBuster.Models
{
    public class GrupoEjercicios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string URLVideo { get; set; }
        public string Instrucciones {  get; set; }
        public virtual ICollection<WodXEjercicio> WodXEjercicio { get; set; }


    }
}
