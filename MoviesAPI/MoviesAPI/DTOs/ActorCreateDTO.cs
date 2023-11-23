using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class ActorCreateDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        [FileWeightValidation(MaximumWeightMegaBytes: 4)] // le pasamos 4 MB maximo
        [TypeFileValidation(groupFileType: GroupFileType.Imagen)] // validamos e inicamos que es imagen
        public IFormFile Photo { get; set; } // IFormFile para recibir la imagen
    }
}
