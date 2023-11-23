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
    }
}
