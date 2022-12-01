using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.DTOs
{
    public class PremioCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string NombrePremio { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 50 carácteres")]
        public string Descripcion { get; set; }
        public int NumPremio { get; set; }
    }
}
