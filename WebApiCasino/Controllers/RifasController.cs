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

        [HttpGet("Lista_Rifas")]      
        public async Task<ActionResult<List<RifaDTO>>> Get()
        {
            var rifa = await dbContext.Rifas.ToListAsync();
            return mapper.Map<List<RifaDTO>>(rifa);
        }

        //[HttpGet("{nombre}")]
        //public async Task<ActionResult<List<RifaDTO>>> Get([FromRoute] string nombre)
        //{
        //    var rifas = await dbContext.Rifas.Where(rifaBD => rifaBD.NombreRifa.Contains(nombre)).ToListAsync();

        //    return mapper.Map<List<RifaDTO>>(rifas);

        //}

        [HttpPost]
        public async Task<ActionResult> Post(RifaCreacionDTO rifaCreacionDTO)
        {
            var existrifa = await dbContext.Rifas.AnyAsync(x => x.Id == rifaCreacionDTO.Id);

            if (existrifa)
            {
                return BadRequest("Ya existe una rifa con el id.");
            }
            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);
            dbContext.Add(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("Modificar_Rifa/{id:int}")]
        public async Task<ActionResult> Put(RifaCreacionDTO rifaCreacionDTO, int id)
        {
            var exist = await dbContext.Rifas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("No se encontró la rifa, intentelo más tarde.");
            }
            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);
            rifa.Id = id;
            if (rifa.Id != id)
            {
                return BadRequest("El id no coincide con el participante, ingreselo correctamente.");
            }
            dbContext.Update(rifa);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("Borrar_Rifa/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existrifa = await dbContext.Rifas.AnyAsync(x => x.Id == id);
            if (!existrifa)
            {
                return NotFound("No se encontró ninguna rifa, ingreselo nuevamente.");
            }
            dbContext.Remove(new Rifa()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}