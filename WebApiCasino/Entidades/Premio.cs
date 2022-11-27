using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.Entidades
{
    public class Premio
    {
        public int id { get; set; }
        public int idRifa { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string premio { get; set; }
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 50 carácteres")]
        public string descripcion { get; set; }
        public int numpremio { get; set; }

        public Rifa Rifa { get; set; }
    }
}