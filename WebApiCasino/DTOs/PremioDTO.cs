using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.DTOs
{
    public class PremioDTO
    {
        public string NombrePremio { get; set; }
        public string Descripcion { get; set; }
        public int NumPremio { get; set; }
    }
}
