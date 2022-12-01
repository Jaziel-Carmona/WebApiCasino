using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using WebApiCasino.DTOs;
using WebApiCasino.Entidades;

namespace WebApiCasino.Controllers
{
    [ApiController]
    [Route("Cartas_de_Rifa")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CartasController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartaDTO>>> Get(int rifaId)
        {
            var existeRifa = await dbContext.Rifas.AnyAsync(rifaDB => rifaDB.Id == rifaId);

            if (!existeRifa)
            {
                return NotFound();
            }

            var cartas = await dbContext.Premios.Where(cartaDB => cartaDB.RifaId == rifaId).ToListAsync();

            return mapper.Map<List<CartaDTO>>(cartas);
        }

        [HttpGet("{id:int}", Name = "obtenerCarta")]
        public async Task<ActionResult<CartaDTO>> GetById(int id)
        {
            var carta = await dbContext.Premios.FirstOrDefaultAsync(cartaDB => cartaDB.Id == id);

            if (carta == null)
            {
                return NotFound();
            }

            return mapper.Map<CartaDTO>(carta);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> Post(int rifaId, PremioCreacionDTO premioCreacionDTO)
        //{
        //    var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

        //    var email = emailClaim.Value;

        //    var usuario = await userManager.FindByEmailAsync(email);
        //    var usuarioId = usuario.Id;

        //    var existeRifa = await dbContext.Rifas.AnyAsync(rifaDB => rifaDB.Id == rifaId);
        //    if (!existeRifa)
        //    {
        //        return NotFound();
        //    }

        //    var premio = mapper.Map<Premios>(premioCreacionDTO);
        //    premio.RifaId = rifaId;
        //    premio.UsuarioId = usuarioId;
        //    dbContext.Add(premio);
        //    await dbContext.SaveChangesAsync();

        //    var premioDTO = mapper.Map<PremioDTO>(premio);

        //    return CreatedAtRoute("obtenerPremio", new { id = premio.Id, premioId = premioId }, premioDTO);
        //}

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int rifaId, int id, CartaCreacionDTO cartaCreacionDTO)
        {
            var existeRifa = await dbContext.Rifas.AnyAsync(rifaDB => rifaDB.Id == rifaId);
            if (!existeRifa)
            {
                return NotFound();
            }

            var existeCarta = await dbContext.Cartas.AnyAsync(cartaDB => cartaDB.Id == id);

            if (!existeCarta)
            {
                return NotFound();
            }

            var carta = mapper.Map<Carta>(cartaCreacionDTO);
            carta.Id = id;

            dbContext.Update(carta);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}