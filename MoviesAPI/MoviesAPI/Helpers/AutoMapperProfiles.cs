using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            // ReverseMap: para poder mappear de Gender a GenderDTO y viceversa
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderCreateDTO, Gender>();

            // ForMember: para ignorar el campo foto, para que no actualice siempre, solo cuando es necesario
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreateDTO, Actor>().ForMember(x => x.Photo, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<MovieCreateDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenders, options => options.MapFrom(MapMoviesGenders))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));
            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }

        private List<MoviesGenders> MapMoviesGenders(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            var result = new List<MoviesGenders>();

            if (movieCreateDTO.GendersIDs == null) { return result; } // si es null retornamos objeto vacio

            // iteramos IDs
            foreach(var id in movieCreateDTO.GendersIDs)
            {
                result.Add(new MoviesGenders() { GenderId = id });
            }

            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreateDTO movieCreateDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            if(movieCreateDTO.Actors == null) { return result; } // si es null retornamos objeto vacio

            // iteramos IDs
            foreach(var actor in movieCreateDTO.Actors)
            {
                result.Add(new MoviesActors() { ActorId = actor.ActorId, Character = actor.Character });
            }

            return result;
        }
    }
}
