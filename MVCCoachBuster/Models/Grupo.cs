namespace MVCCoachBuster.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int WodId { get; set; }
        public Wod Wod { get; set; }


    }
}
