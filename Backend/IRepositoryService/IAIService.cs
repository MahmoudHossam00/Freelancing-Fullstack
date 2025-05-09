using Freelancing.DTOs;

namespace Freelancing.IRepositoryService
{
    public interface IAIService
    {
        Task<List<ProjectForAI_DTO>> GetRecommendedProjectsAsync(List<string> freelancerSkills, List<ProjectForAI_DTO> allProjects);
    }
}
