using WebApiCasino.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("Participantes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParticipantesController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public ParticipantesController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet("(Datos_Del_Participante)/{id:int}", Name = "Datos_Del_Participante")]
        public async Task<ActionResult<GetParticipanteDTO>> GetById([FromRoute] int id)
        {

            var participante = await dbContext.Participantes.FirstOrDefaultAsync(x => x.Id == id);

            if (participante == null)
            {
                return NotFound("No se encontro participante con ese id");
            }


            return mapper.Map<GetParticipanteDTO>(participante);
        }

       

        [HttpPost("Crear_Participante")]
        public async Task<ActionResult> Post(ParticipanteDTO creacionParticipanteDTO)
        {
            var existeCorreo = await dbContext.Participantes.AnyAsync(x => x.Email == creacionParticipanteDTO.Email);

            if (existeCorreo)
            {
                return BadRequest("Ya existe este correo");
            }


            var newParticipante = mapper.Map<Participante>(creacionParticipanteDTO);
            dbContext.Add(newParticipante);

            var partDTO = mapper.Map<GetParticipanteDTO>(newParticipante);
            await dbContext.SaveChangesAsync();

            var creado = await dbContext.Participantes.FirstOrDefaultAsync(participanteDB => participanteDB.Email == creacionParticipanteDTO.Email);
            partDTO.Id = creado.Id;
            return CreatedAtRoute("Datos_Del_Participante", new { Id = creado.Id }, partDTO);
        }

        [HttpPost("Comprar_Carta_De_Rifa/ Participante{idParticipante:int}/Rifa{idRifa:int}/Carta{NumLoteria:int}")]
        public async Task<ActionResult> PostRelacion(int idParticipante, int idRifa, int NumLoteria)
        {
            var existeParticipante = await dbContext.Participantes.AnyAsync(x => x.Id == idParticipante);

            if (!existeParticipante)
            {
                return NotFound("No se encontro un participante con ese Id");
            }

            var existeRifa = await dbContext.Rifas.AnyAsync(x => x.Id == idRifa);

            if (!existeRifa)
            {
                return NotFound("No se encontro una rifa con ese Id");
            }

            var existeCarta = await dbContext.Cartas.AnyAsync(x => x.Id == NumLoteria);

            if (!existeCarta)
            {
                return NotFound("No se encontro un carta con ese Id");
            }

            try
            {
                var relacion = await dbContext.ParticipanteRifaCarta.SingleAsync(x => x.IdParticipante == idParticipante.ToString() && x.IdRifa == idRifa.ToString() && x.IdCarta == NumLoteria.ToString());
                return BadRequest("Ya existe la relacion.");
            }
            catch
            {
                var listaParticipantes = await dbContext.ParticipanteRifaCarta.Where(x => x.IdRifa == idRifa.ToString()).ToListAsync();

                if (!(listaParticipantes.Count() <= 54))
                {
                    return BadRequest("No hay lugar disponible en la rifa");
                }

                var cartaUsada = await dbContext.ParticipanteRifaCarta.AnyAsync(x => x.IdRifa == idRifa.ToString() && x.IdCarta == NumLoteria.ToString());

                if (cartaUsada)
                {
                    return BadRequest("La carta ya esta siendo usada en esta rifa");
                }

                var carta = await dbContext.Cartas.FirstAsync(x => x.Id == NumLoteria);
                var rifa = await dbContext.Rifas.FirstAsync(x => x.Id == idRifa);
                var participante = await dbContext.Participantes.FirstAsync(x => x.Id == idParticipante);
                var nuevaRelacion = new ParticipanteRifaCarta()
                {
                    IdParticipante = idParticipante.ToString(),
                    IdRifa = idRifa.ToString(),
                    IdCarta = NumLoteria.ToString(),
                    Participante = participante,
                    Rifa = rifa,
                    Carta = carta
                };
                dbContext.Add(nuevaRelacion);

                var relacionDTO = mapper.Map<ParticipanteRifaCarta>(nuevaRelacion);
                await dbContext.SaveChangesAsync();

                return Ok();
            }
        }


        //[HttpPatch("Modificar_Participante/{id:int}")]
        //[ServiceFilter(typeof(FiltroRegistro))]
        //public async Task<ActionResult> Patch2(int id, JsonPatchDocument<ParticipantePatchDTO> patchDocument)
        //{

        //    if (patchDocument == null)
        //    {
        //        return BadRequest();
        //    }

        //    var participanteDB = await dbContext.Participantes.FirstOrDefaultAsync(x => x.Id == id);

        //    if (participanteDB == null) { return NotFound(); }

        //    var participanteDTO = mapper.Map<ParticipantePatchDTO>(participanteDB);

        //    patchDocument.ApplyTo(participanteDTO);

        //    var isValid = TryValidateModel(participanteDTO);

        //    if (!isValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    mapper.Map(participanteDTO, participanteDB);
        //    await dbContext.SaveChangesAsync();

        //    return Ok();

        //}


        [HttpDelete("Eliminar_Inscripcion/Participante{idParticipante:int}/Rifa{idRifa:int}/Carta{NumLoteria:int}")]
        public async Task<ActionResult> DeleteRelacion(int idParticipante, int idRifa, int NumLoteria)
        {
            var existeParticipante = await dbContext.Participantes.AnyAsync(x => x.Id == idParticipante);

            if (!existeParticipante)
            {
                return NotFound("No se encontro un participante con ese Id");
            }

            var existeRifa = await dbContext.Rifas.AnyAsync(x => x.Id == idRifa);

            if (!existeRifa)
            {
                return NotFound("No se encontro una rifa con ese Id");
            }

            var existeCarta = await dbContext.Cartas.AnyAsync(x => x.Id == NumLoteria);

            if (!existeCarta)
            {
                return NotFound("No se encontro un carta con ese Id");
            }

            dbContext.Remove(new ParticipanteRifaCarta
            {
                IdParticipante = idParticipante.ToString(),
                IdRifa = idRifa.ToString(),
                IdCarta = NumLoteria.ToString()
            });

            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}