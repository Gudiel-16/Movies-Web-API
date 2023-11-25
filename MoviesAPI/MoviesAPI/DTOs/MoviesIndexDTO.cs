namespace MoviesAPI.DTOs
{
    public class MoviesIndexDTO
    {
        public List<MovieDTO> NextReleases { get; set; }
        public List<MovieDTO> InCinemas { get; set; }
    }
}
