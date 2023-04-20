using RacketsScrapper.Domain.Identity;
using RacketsScrapper.Domain;
using AutoMapper;

namespace IdentityServer
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserDTO, User>().ReverseMap();
        }
    }
}
