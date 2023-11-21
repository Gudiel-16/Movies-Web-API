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

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreateDTO, Actor>();
        }
    }
}
