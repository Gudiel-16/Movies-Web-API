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
            CreateMap<MovieCreateDTO, Movie>().ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }
    }
}
