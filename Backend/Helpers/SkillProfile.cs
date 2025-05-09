using AutoMapper;
using Freelancing.DTOs;

namespace Freelancing.Helpers
{
    public class SkillProfile:Profile

    {
        public SkillProfile()
        {
            CreateMap<Skill, SkillDto>().ReverseMap();
        }
    }
}
