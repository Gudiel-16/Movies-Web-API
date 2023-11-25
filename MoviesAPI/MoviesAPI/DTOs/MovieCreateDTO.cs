using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Helpers;
using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class MovieCreateDTO
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        public bool InCinemas { get; set; } // si esta actual en cine
        public DateTime ReleaseDate { get; set; } // en que fecha se va estrenar

        [FileWeightValidation(MaximumWeightMegaBytes: 4)]
        [TypeFileValidation(GroupFileType.Imagen)]
        public IFormFile Poster { get; set; } // imagen

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))] // TypeBinder de nuestro Helpers, le pasamos tipo de dato en TypeBinder, ya que es generico
        public List<int> GendersIDs { get; set; } // listado de generos de la pelicula, se va mapear con 'MoviesGenders' de 'Movie'

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorsMoviesCreateDTO>>))] // le pasamos tipo de dato en TypeBinder, ya que es generico
        public List<ActorsMoviesCreateDTO> Actors { get; set; } // actores de la pelicula, se va mapear con 'MoviesActors' de 'Movie'
    }
}
