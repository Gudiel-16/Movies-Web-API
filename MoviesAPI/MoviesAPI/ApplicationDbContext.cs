using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;
using System.Reflection.Emit;

namespace MoviesAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // llaves compuestas, para la relacion de muchos a muchos
            modelBuilder.Entity<MoviesActors>().HasKey(x => new { x.ActorId, x.MovieId});

            modelBuilder.Entity<MoviesGenders>().HasKey(x => new { x.GenderId, x.MovieId });

            // Insertando data
            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        // data de prueba
        private void SeedData(ModelBuilder modelBuilder)
        {
            var aventura = new Gender() { Id = 4, Name = "Aventura" };
            var animation = new Gender() { Id = 5, Name = "Animación" };
            var suspenso = new Gender() { Id = 6, Name = "Suspenso" };
            var romance = new Gender() { Id = 7, Name = "Romance" };

            modelBuilder.Entity<Gender>()
                .HasData(new List<Gender>
                {
                    aventura, animation, suspenso, romance
                });

            var jimCarrey = new Actor() { Id = 5, Name = "Jim Carrey", Birthdate = new DateTime(1962, 01, 17), Photo = "null" };
            var robertDowney = new Actor() { Id = 6, Name = "Robert Downey Jr.", Birthdate = new DateTime(1965, 4, 4), Photo = "null" };
            var chrisEvans = new Actor() { Id = 7, Name = "Chris Evans", Birthdate = new DateTime(1981, 06, 13), Photo = "null" };

            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, robertDowney, chrisEvans
                });

            var endgame = new Movie()
            {
                Id = 2,
                Title = "Avengers: Endgame",
                InCinemas = true,
                ReleaseDate = new DateTime(2019, 04, 26),
                Poster = "null"
            };

            var iw = new Movie()
            {
                Id = 3,
                Title = "Avengers: Infinity Wars",
                InCinemas = false,
                ReleaseDate = new DateTime(2019, 04, 26),
                Poster = "null"
            };

            var sonic = new Movie()
            {
                Id = 4,
                Title = "Sonic the Hedgehog",
                InCinemas = false,
                ReleaseDate = new DateTime(2020, 02, 28),
                Poster = "null"
            };
            var emma = new Movie()
            {
                Id = 5,
                Title = "Emma",
                InCinemas = false,
                ReleaseDate = new DateTime(2020, 02, 21),
                Poster = "null"
            };
            var wonderwoman = new Movie()
            {
                Id = 6,
                Title = "Wonder Woman 1984",
                InCinemas = false,
                ReleaseDate = new DateTime(2020, 08, 14),
                Poster = "null"
            };

            modelBuilder.Entity<Movie>()
                .HasData(new List<Movie>
                {
                    endgame, iw, sonic, emma, wonderwoman
                });

            modelBuilder.Entity<MoviesGenders>().HasData(
                new List<MoviesGenders>()
                {
                    new MoviesGenders(){MovieId = endgame.Id, GenderId = suspenso.Id},
                    new MoviesGenders(){MovieId = endgame.Id, GenderId = aventura.Id},
                    new MoviesGenders(){MovieId = iw.Id, GenderId = suspenso.Id},
                    new MoviesGenders(){MovieId = iw.Id, GenderId = aventura.Id},
                    new MoviesGenders(){MovieId = sonic.Id, GenderId = aventura.Id},
                    new MoviesGenders(){MovieId = emma.Id, GenderId = suspenso.Id},
                    new MoviesGenders(){MovieId = emma.Id, GenderId = romance.Id},
                    new MoviesGenders(){MovieId = wonderwoman.Id, GenderId = suspenso.Id},
                    new MoviesGenders(){MovieId = wonderwoman.Id, GenderId = aventura.Id},
                });

            modelBuilder.Entity<MoviesActors>().HasData(
                new List<MoviesActors>()
                {
                    new MoviesActors(){MovieId = endgame.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = endgame.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = iw.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = iw.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = sonic.Id, ActorId = jimCarrey.Id, Character = "Dr. Ivo Robotnik", Order = 1}
                });
        }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenders> MoviesGenders { get; set; }
    }
}
