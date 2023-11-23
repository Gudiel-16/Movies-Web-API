using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Helpers
{
    // Tendra metodos de extension
    public static class HttpContextExtensions
    {
        // HttpContext: para extender HttpContext para agregar cabeceras http
        // IQueryable: para determinar la cantidad total de registros en la tabla
        public async static Task InsertParametersPagination<T>(this HttpContext httpContext, IQueryable<T> queryable, int numberOfRecordsPerPage)
        {
            double number = await queryable.CountAsync(); // hacemos conteo
            double numberOfPage = Math.Ceiling(number / numberOfRecordsPerPage); // cantidad de paginas
            httpContext.Response.Headers.Add("numberOfPage", numberOfPage.ToString()); // agregamos la cantidad en la cabecera
        }
    }
}
