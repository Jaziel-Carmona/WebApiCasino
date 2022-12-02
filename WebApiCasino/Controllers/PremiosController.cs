using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.DTOs;
using WebApiCasino.Entidades;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("Premios")]
    public class PremiosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<ParticipantesController> logger;
        private readonly IWebHostEnvironment env;
        private readonly IMapper mapper;

        public PremiosController(ApplicationDbContext dbContext, ILogger<ParticipantesController> logger, IWebHostEnvironment env, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.env = env;
            this.mapper = mapper;
        }

        //[HttpGet]   
        //[AllowAnonymous]
        //public async Task<ActionResult<List<PremioDTO>>> Get()
        //{
        //    var premio = await dbContext.Premios.ToListAsync();
        //    return mapper.Map<List<PremioDTO>>(premio);
        //}

        [HttpGet("Mostrar_Premios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult<List<Rifa>>> Get(int id)
        {
            var rifa = await dbContext.Rifas.FirstOrDefaultAsync(x => x.Id == id);
            if (rifa == null)
            {
                return NotFound("No se encontró el premio ingresado.");
            }
            var existpremio = await dbContext.Premios.AnyAsync(premioDB => premioDB.RifaId == rifa.Id);
            if (!existpremio)
            {
                return NotFound("No se encontró el premio ingresado.");
            }
            int premioRifaCount = await dbContext.Premios.CountAsync(premioDB => premioDB.RifaId == rifa.Id).ConfigureAwait(false);
            Random premiorandom = new Random();
            int numrandom = premiorandom.Next(0, premioRifaCount);
            var premio = await dbContext.Premios.Skip(numrandom).FirstOrDefaultAsync(premioDB => premioDB.RifaId == rifa.Id);
            return Ok(premio);
        }

        [HttpPost("Ingresar_premio")]  
        public async Task<ActionResult> Post(PremioCreacionDTO premioCreacionDTO)
        {
            var existpremio = await dbContext.Premios.AnyAsync(x => x.NombrePremio == premioCreacionDTO.NombrePremio);
            if (existpremio)
            {
                return BadRequest("Ya existe el premio, intente registrar otro diferente.");
            }
            var premio = mapper.Map<Premios>(premioCreacionDTO);
            dbContext.Add(premio);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("Modificar_premio:{id:int}")]       
        public async Task<ActionResult> Put(PremioCreacionDTO premioCreacionDTO, int id)
        {
            var exist = await dbContext.Premios.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("No se encontró el premio ingresado.");
            }
            var premio = mapper.Map<Premios>(premioCreacionDTO);
            premio.Id = id;
            if (premio.Id != id)
            {
                return BadRequest("El id del premio no coincide, inténtelo de nuevo.");
            }
            dbContext.Update(premio);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("Borrar_Premio/{id:int}")]    //ELIMINACION DE PREMIO
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Premios.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("No se encontró el premio, inténtelo de nuevo.");
            }
            dbContext.Remove(new Premios()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}