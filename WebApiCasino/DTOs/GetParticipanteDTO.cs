using System.ComponentModel.DataAnnotations;
using WebApiCasino.Validaciones;

namespace WebApiCasino.DTOs
{
    public class GetParticipanteDTO
    {
        public int Id { get; set; }

        public string NombreParticipante { get; set; }

        public int Edad { get; set; }
       
        public string Email { get; set; }
        
        public string Telefono { get; set; }
    }
}
