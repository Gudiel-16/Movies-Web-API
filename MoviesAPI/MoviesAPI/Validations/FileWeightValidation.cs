using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations
{
    public class FileWeightValidation: ValidationAttribute
    {
        private readonly int maximumWeightMegaBytes;

        public FileWeightValidation(int MaximumWeightMegaBytes) 
        {
            maximumWeightMegaBytes = MaximumWeightMegaBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // validando peso (valor)
            if(value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            // validando el tipo (si transforma)
            if(formFile == null)
            {
                return ValidationResult.Success;
            }

            // validamos que sea < 4 MB
            if (formFile.Length > maximumWeightMegaBytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe ser mayor a {maximumWeightMegaBytes} MB");
            }

            return ValidationResult.Success;
        }
    }
}
