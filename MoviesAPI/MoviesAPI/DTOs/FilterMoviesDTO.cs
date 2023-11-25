
namespace MoviesAPI.DTOs
{
    public class FilterMoviesDTO
    {
        // paginacion
        public int Page { get; set; } = 1;
        public int NumberOfRecordsPerPage { get; set; } = 10;
        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, numberOfRecordsPerPage = NumberOfRecordsPerPage }; }
        }

        // campos por donde podra filtrar
        public string Title { get; set; } = "";
        public int GenderId { get; set; }
        public bool InCinemas { get; set; }
        public bool NextReleases { get; set; }
    }
}
