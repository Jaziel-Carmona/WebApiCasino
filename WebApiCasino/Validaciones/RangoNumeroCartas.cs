using System.ComponentModel.DataAnnotations;

namespace WebApiCasino.Validaciones
{
    public class RangoNumeroCartas : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valor = Convert.ToInt32(value);

            if (!(valor >= 1) || !(valor <= 54))
            {
                return new ValidationResult("El numero no esta dentro del rango de cartas existentes");
            }

            return ValidationResult.Success;
        }

    }
}