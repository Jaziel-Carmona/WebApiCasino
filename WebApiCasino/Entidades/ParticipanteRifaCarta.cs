namespace WebApiCasino.Entidades
{
    public class ParticipanteRifaCarta
    {
        public string IdParticipante { get; set; }

        public string IdRifa { get; set; }

        public string IdCarta { get; set; }

        public Participante Participante { get; set; }
        public Rifa Rifa { get; set; }
        public Carta Carta { get; set; }
    }
}