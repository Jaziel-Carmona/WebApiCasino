namespace WebApiCasino.Entidades
{
    public class ParticipanteRifaCarta
    {
        public int IdParticipante { get; set; }
        public int IdRifa { get; set; }
        public int IdCarta { get; set; }

        public Participante Participante { get; set; }
        public Rifa Rifa { get; set; }
        public Carta Carta { get; set; }
    }
}