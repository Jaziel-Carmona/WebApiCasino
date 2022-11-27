namespace WebApiCasino.Entidades
{
    public class Participante
    {
        public int id { get; set; }
        public string NombreParticipante { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public string Tarjeta { get; set; }
    }
}