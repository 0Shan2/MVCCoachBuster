namespace MVCCoachBuster.Models
{
    public class Ejercicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Repeticiones { get; set; }

        public int WodId { get; set; }
        public  Wod  wod { get; set; }
   
    }
}
