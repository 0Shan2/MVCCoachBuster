namespace MVCCoachBuster.Models
{
    public class WodXEjercicio
    {
        public int Id { get; set; }
        public int IdWod { get; set; }

        public  Wod Wod { get; set; }

        public int IdGrupoEjercicios { get; set; }
        public  GrupoEjercicios GrupoEjercicios { get; set;}

    }
}
