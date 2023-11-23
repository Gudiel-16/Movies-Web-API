using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;

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

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenders> MoviesGenders { get; set; }
    }
}
