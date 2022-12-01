using WebApiCasino.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApiCasino;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("Participantes_de_Rifa")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class ParticipantesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;


        public ParticipantesController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetParticipanteDTO>>> Get()
        {
            var participantes = await dbContext.Participantes.ToListAsync();
            return mapper.Map<List<GetParticipanteDTO>>(participantes);
        }

        [HttpGet("{id:int}", Name = "obtenerparticipante")]
        public async Task<ActionResult<ParticipanteDTOConRifas>> Get(int id)
        {
            var participante = await dbContext.Participantes
                .Include(participanteDB => participanteDB.ParticipanteRifaCarta)
                .ThenInclude(participanterifaCartaDB => participanterifaCartaDB.Rifa)
                .FirstOrDefaultAsync(participanteBD => participanteBD.Id == id);

            if (participante == null)
            {
                return NotFound();
            }

            return mapper.Map<ParticipanteDTOConRifas>(participante);

        }

        [HttpGet("obtenerParticipante/{nombre}")]
        public async Task<ActionResult<List<GetParticipanteDTO>>> Get([FromRoute] string nombre)
        {
            var participante = await dbContext.Participantes.Where(participanteBD => participanteBD.NombreParticipante.Contains(nombre)).ToListAsync();

            return mapper.Map<List<GetParticipanteDTO>>(participante);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ParticipanteDTO participanteDto)
        {

            var existeParticipanteMismoNombre = await dbContext.Participantes.AnyAsync(x => x.NombreParticipante == participanteDto.NombreParticipante);

            if (existeParticipanteMismoNombre)
            {
                return BadRequest($"Ya existe un participante con el nombre {participanteDto.NombreParticipante}");
            }

            var participante = mapper.Map<Participante>(participanteDto);

            dbContext.Add(participante);
            await dbContext.SaveChangesAsync();

            var participanteDTO = mapper.Map<GetParticipanteDTO>(participante);

            return CreatedAtRoute("obtenerparticipante", new { id = participante.Id }, participanteDTO);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ParticipanteDTO participanteCreacionDTO, int id)
        {
            var exist = await dbContext.Participantes.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var participante = mapper.Map<Participante>(participanteCreacionDTO);
            participante.Id = id;

            dbContext.Update(participante);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Participantes.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El registro del participante no fue encontrado.");
            }

            dbContext.Remove(new Participante()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}