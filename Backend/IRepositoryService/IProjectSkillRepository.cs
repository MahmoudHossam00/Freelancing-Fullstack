namespace Freelancing.IRepositoryService
{
    public interface IProjectSkillRepository
    {
        Task<List<ProjectSkill>> GetAllAsync();
        Task<ProjectSkill?> GetByIdAsync(int id);
        Task<ProjectSkill> CreateProjectSkill(ProjectSkill projectSkill);
        Task<ProjectSkill?> UpdateAsync(ProjectSkill projectSkill);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int projectId, int skillId);
        Task<bool> ProjectExistsAsync(int projectId);
        Task<bool> SkillExistsAsync(int skillId);
    }
}
