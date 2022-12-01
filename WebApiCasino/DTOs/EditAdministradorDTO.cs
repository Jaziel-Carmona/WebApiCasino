using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.DTOs
{
    public class EditAdministradorDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}