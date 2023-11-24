using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MoviesAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder // generico, para que reciba el tipo de dato a operar
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var valuesProvider = bindingContext.ValueProvider.GetValue(propertyName);
            
            if(valuesProvider == ValueProviderResult.None) { return Task.CompletedTask; }

            try
            {
                //Console.WriteLine(valuesProvider.ToString());
                // desealizar
                var valueDeserialize = JsonConvert.DeserializeObject<T>(valuesProvider.FirstValue); // desealizar el generico
                bindingContext.Result = ModelBindingResult.Success(valueDeserialize); // listado que envian
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(propertyName, "Valor invalido para tipo <T>");
            }

            return Task.CompletedTask;
        }
    }
}
