using AutoMapper;
using RacketsScrapper.Domain;
using RacketsScrapper.Domain.Identity;

namespace RacketScrapper.API
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
