namespace WebApiCasino.DTOs
{
    public class ParticipanteDTOConRifas : GetParticipanteDTO
    {
        public List<RifaDTO> Rifas { get; set; }
    }
}
