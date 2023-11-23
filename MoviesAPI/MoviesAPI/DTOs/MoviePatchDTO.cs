using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool InCinemas { get; set; } // si esta actual en cine
        public DateTime ReleaseDate { get; set; } // en que fecha se va estrenar
    }
}
