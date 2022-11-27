namespace WebApiCasino.Entidades
{
    public class TarjetaGanador
    {
        public int idRifa { get; set; }
        public string NombreRifa { get; set; }
        public string premio { get; set; }
        public string NombreParticipante { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }

        public Rifa Rifa { get; set; }
        public Premio Premio { get; set; }
        public Participante Participante { get; set; }
    }
}