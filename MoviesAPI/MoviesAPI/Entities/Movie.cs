using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool InCinemas { get; set; } // si esta actual en cine
        public DateTime ReleaseDate { get; set; } // en que fecha se va estrenar
        public string Poster { get; set; } // URL al poster de la pelicula
        public List<MoviesActors> MoviesActors { get; set; } // navegacion
        public List<MoviesGenders> MoviesGenders { get; set; } // navegacion
    }
}
