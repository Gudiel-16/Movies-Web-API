using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string storage = "movies"; // nombre de carpeta en Azure o Local (imagenes)

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get()
        {
            var movies = await context.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null) { return NotFound(); }

            return mapper.Map<MovieDTO>(movie);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movie = mapper.Map<Movie>(movieCreateDTO);

            // guardando foto
            if (movieCreateDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreateDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreateDTO.Poster.FileName);
                    movie.Poster = await fileStorage.saveFile(content, extension, storage, movieCreateDTO.Poster.ContentType);
                }
            }

            context.Add(movie);
            await context.SaveChangesAsync();

            var movieDTO = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, movieDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreateDTO movieCreateDTO)
        {
            var movieDB = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (movieDB == null) { return NotFound(); }

            // para que solo actualize los campos enviados, campo poster es ignorado, configurado en AutoMapperProfiles
            movieDB = mapper.Map(movieCreateDTO, movieDB);

            // guardando foto
            if (movieCreateDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreateDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreateDTO.Poster.FileName);
                    movieDB.Poster = await fileStorage.editFile(content, extension, storage, movieDB.Poster, movieCreateDTO.Poster.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            if (patchDocument == null) { return BadRequest(); }

            var entityDB = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (entityDB == null) { return NotFound(); }

            // lleno ActorPatchDTO con la informacion de entityDB
            var entityDTO = mapper.Map<MoviePatchDTO>(entityDB);

            // aplico a ActorPatchDTO los cambios que vinieron en el patchDocument, por ejemplo solo el name
            patchDocument.ApplyTo(entityDTO, ModelState); // en modelState se guardan los errores

            var isValid = TryValidateModel(entityDTO);

            if (!isValid) { return BadRequest(); }

            mapper.Map(entityDTO, entityDB);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            // AnyAsync: si existe alguna entidad con la propiedad id (valor)
            var exist = await context.Movies.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Movie() { Id = id }); // basta solo con el id para eliminar
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
