using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string storage = "actors"; // nombre de carpeta en Azure (imagenes)

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage) {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            // ToListAsync: retorna todos los actores
            var entities = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entities); // Map de Actor a ActorDTO
        }

        [HttpGet("{id}", Name = "getActor")]
        public async Task<ActionResult<ActorDTO>> get(int id)
        {
            // FirstOrDefaultAsync: retornar el primero que coincida con ese id
            var entity = context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if(entity == null) { return NotFound(); }

            return mapper.Map<ActorDTO>(entity);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreateDTO actorCreateDTO)
        {
            var entity = mapper.Map<Actor>(actorCreateDTO);

            // guardando foto en azure
            if(actorCreateDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreateDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreateDTO.Photo.FileName);
                    entity.Photo = await fileStorage.saveFile(content, extension, storage, actorCreateDTO.Photo.ContentType);
                }
            }

            context.Add(entity);
            await context.SaveChangesAsync();

            var dto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("getActor", new { id = entity.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreateDTO actorCreateDTO)
        {
            var actorDB = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            
            if (actorDB == null) { return NotFound(); }

            // para que solo actualize los campos enviados, campo photo es ignorado, configurado en AutoMapperProfiles
            actorDB = mapper.Map(actorCreateDTO, actorDB);

            // guardando foto en azure
            if (actorCreateDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreateDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreateDTO.Photo.FileName);
                    actorDB.Photo = await fileStorage.editFile(content, extension, storage, actorDB.Photo, actorCreateDTO.Photo.ContentType);
                }
            }

            // actualizar todos los campos (antes 1.0)
            //var entity = mapper.Map<Actor>(actorCreateDTO);
            //entity.Id = id;
            //context.Entry(entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if(patchDocument == null) { return BadRequest(); }

            var entityDB = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if(entityDB == null) { return NotFound(); }

            // lleno ActorPatchDTO con la informacion de entityDB
            var entityDTO = mapper.Map<ActorPatchDTO>(entityDB);

            // aplico a ActorPatchDTO los cambios que vinieron en el patchDocument, por ejemplo solo el name
            patchDocument.ApplyTo(entityDTO, ModelState); // en modelState se guardan los errores

            var isValid = TryValidateModel(entityDTO);

            if(!isValid) { return BadRequest(); }

            mapper.Map(entityDTO, entityDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // AnyAsync: si existe alguna entidad con la propiedad id (valor)
            var exist = await context.Actors.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Actor() { Id = id }); // basta solo con el id para eliminar
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
