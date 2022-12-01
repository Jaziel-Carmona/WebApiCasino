using WebApiCasino.Validaciones;

namespace WebApiCasino.DTOs
{
    public class CartaDTO
    {
        public int Id { get; set; }

        [RangoNumeroCartas]
        public int NumeroCarta { get; set; }
    }
}
