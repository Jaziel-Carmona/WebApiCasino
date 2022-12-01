﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebApiCasino.Validaciones;

namespace WebApiCasino.Entidades
{
    public class Participante
    {
        public int Id { get; set; }

        [Required]
        [PrimerLetraMayuscula]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} solo puede tener máximo 20 carácteres")]
        public string NombreParticipante { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }

        public List<ParticipanteRifaCarta> ParticipanteRifaCarta { get; set; }


    }
}