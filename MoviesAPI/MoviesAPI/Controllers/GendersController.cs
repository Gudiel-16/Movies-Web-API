using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GendersController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GendersController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> Get()
        {
            var entities = await context.Genders.ToListAsync();
            var dtos = mapper.Map<List<GenderDTO>>(entities); // Map de Gender a GenderDTO
            return dtos;
        }

        [HttpGet("{id:int}", Name = "getGender")]
        public async Task<ActionResult<GenderDTO>> Get(int id)
        {
            // FirstOrDefaultAsync: retornar el primero que coincida con ese id
            var entity = await context.Genders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<GenderDTO>(entity);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenderCreateDTO genderCreateDTO)
        {
            var entity = mapper.Map<Gender>(genderCreateDTO); // de genderCreateDTO a Gender
            context.Add(entity);
            await context.SaveChangesAsync();

            var genderDTO = mapper.Map<GenderDTO>(entity); // de Gender a GenderDTO

            // getGender: nombre de la ruta para obtener, esta ruta recibe un id
            return new CreatedAtRouteResult("getGender", new { id = genderDTO.Id }, genderDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenderCreateDTO genderCreateDTO)
        {
            var entity = mapper.Map<Gender>(genderCreateDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified; // indicamos que se modifico
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // AnyAsync: si existe alguna entidad con la propiedad id (valor)
            var exist = await context.Genders.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Gender() { Id = id }); // basta solo con el id
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
