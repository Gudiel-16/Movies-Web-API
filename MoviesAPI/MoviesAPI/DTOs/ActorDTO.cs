using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Photo { get; set; }
    }
}
