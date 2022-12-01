using WebApiCasino.Validaciones;

namespace WebApiCasino.DTOs
{
    public class CartaCreacionDTO
    {
        [RangoNumeroCartas]
        public int NumeroCarta { get; set; }
    }
}
