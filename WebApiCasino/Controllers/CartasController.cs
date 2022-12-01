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
    [Route("Cartas_de_Loteria")]
    public class CartasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;


        public CartasController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet("Cartas_Por_Rifa/{id:int}/{nombreRifa}")]
        public async Task<ActionResult<List<CartaDTO>>> GetById(int id, string nombreRifa)
        {

            var existe = await dbContext.Rifas.AnyAsync(x => x.Id == id && x.NombreRifa == nombreRifa);

            if (!existe)
            {
                return BadRequest("No existe la rifa a la que se desea acceder");
            }

            var cartas = await dbContext.Cartas.ToListAsync();
            var borrar = await dbContext.Cartas.ToListAsync();
            var relaciones = await dbContext.ParticipanteRifaCarta.Where(c => c.IdRifa == id.ToString()).ToListAsync();
            foreach (var i in relaciones)
            {
                int num = Int32.Parse(i.IdCarta);
                num = num - 1;
                cartas.Remove(borrar[num]);
            }
    
            return mapper.Map<List<CartaDTO>>(cartas);
        }

    }
}