using MoviesAPI.DTOs;

namespace MoviesAPI.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                // es para saltar registros, esta es la funcion de la paginacion
                // si el usuario se encuentra en la pagina 1, no se salta ningun registro: (1-1) * 10 = 0
                // si el usuario se encuentra en la pagina 2, (2-1) * 10 = 10; osea que se salta 10 registros y asi sucesivamente
                .Skip((paginationDTO.Page - 1) * paginationDTO.NumberOfRecordsPerPage)
                .Take(paginationDTO.numberOfRecordsPerPage); // toma 10 registros
        }
    }
}
