using System.ComponentModel.DataAnnotations;
using WebApiCasino.Validaciones;

namespace WebApiCasino.Entidades
{
    public class Carta
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El siguiente campo es obligatorio.")]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string NombreCarta { get; set; }

        public Carta(int id, string NombreCarta)
        {
            this.id = id;
            this.NombreCarta = NombreCarta;
        }
    }
}