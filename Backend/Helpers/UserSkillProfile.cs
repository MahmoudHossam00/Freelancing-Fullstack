using AutoMapper;
using Freelancing.DTOs;

namespace Freelancing.Helpers
{
    public class UserSkillProfile: Profile
    {
        public UserSkillProfile()
        {
            CreateMap<UserSkill, UserSkillDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));

            CreateMap<UserSkillDto, UserSkill>()
                .ForMember(dest => dest.Skill, opt => opt.Ignore());



        }
    }
}
