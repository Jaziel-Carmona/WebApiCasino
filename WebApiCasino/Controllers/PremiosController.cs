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
    [Route("Premios_de_Rifa")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PremiosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public PremiosController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PremioDTO>>> Get(int rifaId)
        {
            var existeRifa = await dbContext.Rifas.AnyAsync(rifaDB => rifaDB.Id == rifaId);

            if (!existeRifa)
            {
                return NotFound();
            }

            var premios = await dbContext.Premios.Where(premioDB => premioDB.RifaId == rifaId).ToListAsync();

            return mapper.Map<List<PremioDTO>>(premios);
        }

        [HttpGet("{id:int}", Name = "obtenerPremio")]
        public async Task<ActionResult<PremioDTO>> GetById(int id)
        {
            var premio = await dbContext.Premios.FirstOrDefaultAsync(premioDB => premioDB.Id == id);

            if (premio == null)
            {
                return NotFound();
            }

            return mapper.Map<PremioDTO>(premio);
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
        public async Task<ActionResult> Put(int rifaId, int id, PremioCreacionDTO premioCreacionDTO)
        {
            var existeRifa = await dbContext.Rifas.AnyAsync(rifaDB => rifaDB.Id == rifaId);
            if (!existeRifa)
            {
                return NotFound();
            }

            var existePremio = await dbContext.Premios.AnyAsync(premioDB => premioDB.Id == id);

            if (!existePremio)
            {
                return NotFound();
            }

            var premio = mapper.Map<Premios>(premioCreacionDTO);
            premio.Id = id;
            premio.RifaId = rifaId;

            dbContext.Update(premio);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
