using WebApiCasino.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("Participantes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParticipantesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ParticipantesController(ApplicationDbContext dbContext, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet("Buscar_Participante/{id:int}")]   
        public async Task<ActionResult<GetParticipanteDTO>> Get(int id)
        {
            var participante = await dbContext.Participantes
                .Include(participanteDB => participanteDB.ParticipanteRifaCarta)
                .ThenInclude(participanterifaDB => participanterifaDB.Rifa)
                .FirstOrDefaultAsync(participanteBD => participanteBD.Id == id);
            if (participante == null)
            {
                return NotFound("No hay participantes registrados");
            }
            return mapper.Map<ParticipanteDTOConRifas>(participante);
        }

        [HttpGet("Buscar_por_nombre/{nombre}")]       
        public async Task<ActionResult<List<GetParticipanteDTO>>> Get([FromRoute] string nombre)
        {
            var participantes = await dbContext.Participantes.Where(participantesBD => participantesBD.NombreParticipante.Contains(nombre)).ToListAsync();
            return mapper.Map<List<GetParticipanteDTO>>(participantes);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ParticipanteDTO participanteDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            var existeCliente = await dbContext.Participantes.AnyAsync(x => x.Id == participanteDTO.Id);
            if (existeCliente)
            {
                return BadRequest("Ya existe un cliente con ese id.");
            }
            var cantidadParticipantes = await dbContext.Participantes.CountAsync();
            if (cantidadParticipantes >= 54)
            {
                return BadRequest("Esta rifa se encuentra llena, intente con otra.");
            }
            var participante = mapper.Map<Participante>(participanteDTO);
            participante.UsuarioId = usuarioId;
            dbContext.Add(participante);
            await dbContext.SaveChangesAsync();
            var clienteDTO = mapper.Map<GetParticipanteDTO>(participante);
            return CreatedAtRoute("obtenerParticipante", new { id = participante.Id }, participanteDTO);
        }


        [HttpPut("Modificar_datos_particpante/{id:int}")]            
        public async Task<ActionResult> Put(ParticipanteDTO paticipanteCreacionDTO, int id)
        {
            var exist = await dbContext.Participantes.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            var participante = mapper.Map<Participante>(paticipanteCreacionDTO);
            participante.Id = id;
            dbContext.Update(participante);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Eliminar_Particpante/{id:int}")]            
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Participantes.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Participante no fue encontrado.");
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