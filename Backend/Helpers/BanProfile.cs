using AutoMapper;
using Freelancing.DTOs;

namespace Freelancing.Helpers
{
    public class BanProfile : Profile
    {
        public BanProfile()
        {
            CreateMap<Ban, BanDto>()
                   .ForMember(dest => dest.BannedUserName, opt => opt.MapFrom(src => src.BannedUser.UserName)); 

            CreateMap<BanDto, Ban>()
                .ForMember(dest => dest.BannedUser, opt => opt.Ignore());
        }
    
    }
}
