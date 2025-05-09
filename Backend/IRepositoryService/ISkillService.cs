namespace Freelancing.IRepositoryService
{
    public interface ISkillService
    {
        Task<List<Skill>> GetAllSkillsAsync();
        Task<Skill> GetSkillByIDAsync(int skillId);
        Task<Skill> CreateSkillAsync(Skill skill);
        Task<Skill> UpdateSkillAsync(Skill skill);
        Task<bool> DeleteSkillAsync(int id);
    }
}
