namespace WebApiCasino.Entidades
{
    public class Rifa
    {
        public int id { get; set; }
        public string NombreRifa { get; set; }
        public bool ExistenciaCarta { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaRifa { get; set; }
    }
}