using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.DTOs
{
    public class RifaDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string NombreRifa { get; set; }

        public DateTime FechaRifa { get; set; }
    }
}
