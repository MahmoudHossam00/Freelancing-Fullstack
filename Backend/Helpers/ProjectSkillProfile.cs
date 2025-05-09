using AutoMapper;
using Freelancing.DTOs;

namespace Freelancing.Helpers
{
    public class ProjectSkillProfile:Profile
    {
        public ProjectSkillProfile()
        {
            CreateMap<ProjectSkill, ProjectSkillDto>()
                  .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));

            CreateMap<ProjectSkillCreateDto, ProjectSkill>();
            CreateMap<ProjectSkillUpdateDto, ProjectSkill>();
        }
    }
}
