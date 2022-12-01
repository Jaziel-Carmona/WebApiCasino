using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.Entidades;
using WebApiCasino.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("Rifas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
    public class RifasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<ParticipantesController> logger;
        private readonly IWebHostEnvironment env;
        private readonly IMapper mapper;

        public RifasController(ApplicationDbContext dbContext, ILogger<ParticipantesController> logger, IWebHostEnvironment env, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.env = env;
            this.mapper = mapper;
        }

        [HttpGet("Obtener_Rifa_Con_Premios/Rifa/{id:int}", Name = "GetPremiosRifa")]
        public async Task<ActionResult<RifaDTOConPremios>> GetPremiosRifaId([FromRoute] int id)
        {
            var rifa = await dbContext.Rifas
                .Include(x => x.Premios)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (rifa == null)
            {
                return NotFound("No se encontro una rifa con el Id ingresado");
            }

            List<PremioDTO> listapremios = new List<PremioDTO>();

            foreach (var i in rifa.Premios)
            {
                listapremios.Add(new PremioDTO
                {
                    NombrePremio = i.NombrePremio,
                    NumPremio = i.NumPremio,
                });
            }

            var rifapremio = new RifaDTOConPremios()
            {
                NombreRifa = rifa.NombreRifa,
                Fecha_Realizacion = rifa.Fecha_Realizacion,
                PremioDTO = listapremios
            };

            logger.LogInformation("Se obtiene el listado de premios de una rifa.");
            return mapper.Map<RifaDTOConPremios>(rifapremio);

        }

        [HttpPost("CrearUnaRifa")]
        public async Task<ActionResult> Post(RifaCreacionDTO rifaCreacionDTO)
        {
            var existe = await dbContext.Rifas.AnyAsync(x => x.NombreRifa == rifaCreacionDTO.NombreRifa);

            if (existe)
            {
                return BadRequest("Ya existe una rifa con este nombre");
            }

            var nuevoElemento = mapper.Map<Rifa>(rifaCreacionDTO);

            dbContext.Add(nuevoElemento);


            await dbContext.SaveChangesAsync();

            var rifaDTO = mapper.Map<RifaDTO>(nuevoElemento);

            return CreatedAtRoute("GetPremiosRifa", new { id = nuevoElemento.Id }, rifaDTO);

        }

        [HttpPut("ModificarRifa/{id:int}")]
        public async Task<ActionResult> Put(RifaCreacionDTO creacionRifaDTO, int id)
        {
            var existeRifa = await dbContext.Rifas.AnyAsync(x => x.Id == id);

            if (!existeRifa)
            {
                return BadRequest("La rifa no existe");
            }

            var rifa = mapper.Map<Rifa>(creacionRifaDTO);
            rifa.Id = id;

            dbContext.Update(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("EliminarRifa/{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var existe = await dbContext.Rifas.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound("No se encontro una rifa con ese id");
            }

            var relaciones = await dbContext.ParticipanteRifaCarta.Where(c => c.IdRifa == id.ToString()).ToListAsync();


            foreach (var i in relaciones)
            {
                dbContext.Remove(i);
                await dbContext.SaveChangesAsync();
            }

            var rifa = await dbContext.Rifas.Include(x => x.Premios).FirstAsync(x => x.Id == id);
            dbContext.Remove(rifa);

            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}

