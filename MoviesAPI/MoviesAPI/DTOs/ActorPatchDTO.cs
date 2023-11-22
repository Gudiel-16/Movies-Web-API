using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
