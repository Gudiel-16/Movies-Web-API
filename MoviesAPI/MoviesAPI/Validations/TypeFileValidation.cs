using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations
{
    public class TypeFileValidation: ValidationAttribute
    {
        private readonly string[] validTypes;

        public TypeFileValidation(string[] validTypes) 
        {
            this.validTypes = validTypes;
        }

        public TypeFileValidation(GroupFileType groupFileType) 
        {
            if(groupFileType == GroupFileType.Imagen) // si es imagen, asignamos los tipos aceptables
            {
                validTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // validando tipo (valor)
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            // validando el tipo (si transforma)
            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            // validamos que el tipo sea el correcto (tipo de imagen correcto)
            if (!validTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo del archivo deben ser uno de los siguientes: {string.Join(", ", validTypes)} MB");
            }

            return ValidationResult.Success;
        }
    }
}
