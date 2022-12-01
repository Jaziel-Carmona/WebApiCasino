using System.ComponentModel.DataAnnotations;
using WebApiCasino.Validaciones;

namespace WebApiCasino.Entidades
{
    public class Carta
    {
        public int Id { get; set; }

        [RangoNumeroCartas]
        public int NumeroCarta { get; set; }

        public List<ParticipanteRifaCarta> ParticipanteRifaCarta { get; set; }
    }
}