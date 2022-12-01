using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApiCasino.Entidades
{
    public class Premios
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string NombrePremio { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 50 carácteres")]
        public string Descripcion { get; set; }
        public int NumPremio { get; set; }

        public int RifaId { get; set; }

        public Rifa Rifa { get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario { get; set; }

    }
}