using AutoMapper;
using WebApiCasino.DTOs;
using WebApiCasino.Entidades;

namespace WebApiCasino.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ParticipanteDTO, Participante>();
            CreateMap<CartaCreacionDTO, Carta>();
            CreateMap<Participante, GetParticipanteDTO>();
            CreateMap<Carta, CartaDTO>();
            CreateMap<Participante, ParticipanteDTOConRifas>()
                .ForMember(participanteDTO => participanteDTO.Rifas, opciones => opciones.MapFrom(MapParticipanteDTORifas));
            CreateMap<RifaCreacionDTO, Rifa>()
                .ForMember(rifa => rifa.ParticipanteRifaCarta, opciones => opciones.MapFrom(MapParticipanteRifaCarta));
            CreateMap<Rifa, RifaDTO>();
            CreateMap<RifaCreacionDTO, Rifa>();
            CreateMap<Rifa, RifaDTOConParticipantes>()
                .ForMember(rifaDTO => rifaDTO.Participantes, opciones => opciones.MapFrom(MapRifaDTOParticipantes));
            CreateMap<RifaPatchDTO, Rifa>().ReverseMap();
            CreateMap<Rifa, RifaDTOConCartas>()
                .ForMember(x => x.CartaDTO, opciones => opciones.MapFrom(MapRifaDTOConCartas));
            CreateMap<Rifa, RifaDTOConPremios>()
                .ForMember(x => x.PremioDTO, opciones => opciones.MapFrom(MapRifaDTOPremios));
            CreateMap<PremioCreacionDTO, Premios>();
            CreateMap<Premios, PremioDTO>();

        }

        private List<RifaDTO> MapParticipanteDTORifas(Participante participante, GetParticipanteDTO getParticipanteDTO)
        {
            var result = new List<RifaDTO>();

            if (participante.ParticipanteRifaCarta == null) { return result; }

            foreach (var participanterifaCarta in participante.ParticipanteRifaCarta)
            {
                result.Add(new RifaDTO()
                {
                    NombreRifa = participanterifaCarta.Rifa.NombreRifa,
                    Fecha_Realizacion = participanterifaCarta.Rifa.Fecha_Realizacion
                });
            }

            return result;
        }

        private List<GetParticipanteDTO> MapRifaDTOParticipantes(Rifa rifa, RifaDTO rifaDTO)
        {
            var result = new List<GetParticipanteDTO>();

            if (rifa.ParticipanteRifaCarta == null)
            {
                return result;
            }

            foreach (var participanterifacarta in rifa.ParticipanteRifaCarta)
            {
                result.Add(new GetParticipanteDTO()
                {
                    Id = participanterifacarta.IdParticipante,
                    NombreParticipante = participanterifacarta.Participante.NombreParticipante
                });
            }

            return result;
        }

        private List<ParticipanteRifaCarta> MapParticipanteRifaCarta(RifaCreacionDTO rifaCreacionDTO, Rifa rifa)
        {
            var resultado = new List<ParticipanteRifaCarta>();

            if (rifaCreacionDTO.ParticipantesIds == null) { return resultado; }
            foreach (var participanteId in rifaCreacionDTO.ParticipantesIds)
            {
                resultado.Add(new ParticipanteRifaCarta() { IdParticipante = participanteId });
            }
            return resultado;
        }

        private List<CartaDTO> MapRifaDTOConCartas(Rifa rifa, RifaDTOConCartas RifaDTOconCartas)
        {
            var lista = new List<CartaDTO>();

            if (rifa.ParticipanteRifaCarta == null)
            {
                return lista;
            }
            foreach (var x in rifa.ParticipanteRifaCarta)
            {
                lista.Add(new CartaDTO { NumeroCarta = x.Carta.NumeroCarta });
            }
            return lista;
        }

        private List<PremioDTO> MapRifaDTOPremios(Rifa rifa, RifaDTOConPremios RifaDTOconPremios)
        {
            var lista = new List<PremioDTO>();

            if (rifa.Premios == null)
            {
                return lista;
            }
            foreach (var premios in rifa.Premios)
            {
                lista.Add(new PremioDTO { NumPremio = premios.NumPremio, Descripcion = premios.Descripcion });
            }
            return lista;
        }
    }
}