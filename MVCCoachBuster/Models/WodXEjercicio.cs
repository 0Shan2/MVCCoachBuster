namespace MVCCoachBuster.Models
{
    public class WodXEjercicio
    {
        public int Id { get; set; }
        public int WodId { get; set; }

        public  Wod Wod { get; set; }

        public int GrupoEjerciciosId { get; set; }
        public  GrupoEjercicios GrupoEjercicios { get; set;}

    }
}
