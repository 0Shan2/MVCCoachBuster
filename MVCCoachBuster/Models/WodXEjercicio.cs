namespace MVCCoachBuster.Models
{
    public class WodXEjercicio
    {
        public int Id { get; set; }
        public int WodId { get; set; }

        public virtual Wod wod { get; set; }

        public int EjercicioId { get; set; }
        public virtual Ejercicio ejercicio { get; set;}

    }
}
