namespace MoviesAPI.Entities
{
    public class MoviesActors
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        public string Character { get; set; } // personaje
        public int Order { get; set; } // Orden para mostrar los personajes
        public Actor Actor { get; set; } // navegacion
        public Movie Movie { get; set; } // navegacion

    }
}
