namespace MoviesAPI.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool InCinemas { get; set; } // si esta actual en cine
        public DateTime ReleaseDate { get; set; } // en que fecha se va estrenar
        public string Poster { get; set; } // URL al poster de la pelicula
    }
}
