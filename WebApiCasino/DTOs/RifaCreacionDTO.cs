using System.ComponentModel.DataAnnotations;
using WebApiCasino.Entidades;

namespace WebApiCasino.DTOs
{
    public class RifaCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string NombreRifa { get; set; }

        public DateTime Fecha_Realizacion { get; set; }

        public List<int> ParticipantesIds { get; set; }

    }
}
