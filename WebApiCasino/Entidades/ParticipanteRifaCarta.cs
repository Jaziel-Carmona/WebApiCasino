namespace WebApiCasino.Entidades
{
    public class ParticipanteRifaCarta
    {
        public int idParticipante { get; set; }
        public int idRifa { get; set; }
        public int idCarta { get; set; }

        public Participante Participante { get; set; }
        public Rifa Rifa { get; set; }
        public Carta Carta { get; set; }
    }
}