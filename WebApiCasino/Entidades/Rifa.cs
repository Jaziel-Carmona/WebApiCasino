using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.Entidades
{
    public class Rifa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string NombreRifa { get; set; }

        public DateTime FechaRifa { get; set; }

        public List<Premios> Premios { get; set; }

        public List<ParticipanteRifaCarta> ParticipanteRifaCarta { get; set; }

        public string UsuarioId { get; set; }

        public IdentityUser Usuario { get; set; }
    }
}