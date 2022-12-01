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
    public class RifasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        public RifasController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Rifa>>> GetAll()
        {
            return await dbContext.Rifas.ToListAsync();
        }


        //[HttpGet("{id:int}", Name = "obtenerRifa")]
        ////[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        //public async Task<ActionResult<RifaDTOConParticipantes>> GetById(int id)
        //{
        //    var rifa = await dbContext.Rifas
        //        .Include(rifaDB => rifaDB.ParticipanteRifaCarta)
        //        .ThenInclude(participanterifaCartaDB => participanterifaCartaDB.Participante)
        //        .Include(premioDB => premioDB.Premios)
        //        .FirstOrDefaultAsync(x => x.Id == id);

        //    if (rifa == null)
        //    {
        //        return NotFound();
        //    }

        //    rifa.ParticipanteRifaCarta = rifa.ParticipanteRifaCarta.OrderBy(x => x.Orden_Lanzamiento).ToList();

        //    return mapper.Map<RifaDTOConParticipantes>(rifa);
        //}

        [HttpPost]
        public async Task<ActionResult> Post(RifaCreacionDTO rifaCreacionDTO)
        {

            if (rifaCreacionDTO.ParticipantesIds == null)
            {
                return BadRequest("No se puede crear una rifa sin participantes.");
            }

            var participantesIds = await dbContext.Participantes
                .Where(participanteBD => rifaCreacionDTO.ParticipantesIds.Contains(participanteBD.Id)).Select(x => x.Id).ToListAsync();

            if (rifaCreacionDTO.ParticipantesIds.Count != participantesIds.Count)
            {
                return BadRequest("No existe existe uno de los participantes registrados");
            }

            var rifa = mapper.Map<Rifa>(rifaCreacionDTO);

            //OrdenarPorParticipantes(rifa);

            dbContext.Add(rifa);
            await dbContext.SaveChangesAsync();

            var rifaDTO = mapper.Map<RifaDTO>(rifa);

            return CreatedAtRoute("obtenerRifa", new { id = rifa.Id }, rifaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, RifaCreacionDTO rifaCreacionDTO)
        {
            var rifaDB = await dbContext.Rifas
                .Include(x => x.ParticipanteRifaCarta)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (rifaDB == null)
            {
                return NotFound();
            }

            rifaDB = mapper.Map(rifaCreacionDTO, rifaDB);

            //OrdenarPorParticipantes(rifaDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Rifas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("La Rifa no fue encontrada.");
            }

            //var validateRelation = await dbContext.ArtistaCancion.AnyAsync   


            dbContext.Remove(new Rifa { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        //private void OrdenarPorparticpantes(Rifa rifa)
        //{
        //    if (rifa.ParticipanteRifaCarta != null)
        //    {
        //        for (int i = 0; i < rifa.ParticipanteRifaCarta.Count; i++)
        //        {
        //            rifa.ParticipanteRifaCarta[i].Orden_Lanzamiento = i;
        //        }
        //    }
        //}

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<RifaPatchDTO> patchDocument)
        {
            if (patchDocument == null) { return BadRequest(); }

            var rifaDB = await dbContext.Rifas.FirstOrDefaultAsync(x => x.Id == id);

            if (rifaDB == null) { return NotFound(); }

            var rifaDTO = mapper.Map<RifaPatchDTO>(rifaDB);

            patchDocument.ApplyTo(rifaDTO);

            var isValid = TryValidateModel(rifaDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(rifaDTO, rifaDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}

