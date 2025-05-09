using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IUserSkillService
    {
        Task<List<UserSkill>> GetAllUserSkillAsync();
        Task<UserSkill> GetUserSkillByIDAsync(int id);
        Task<UserSkill> UpdateUserSkillAsync(UserSkill userskill,string freelancerId);
        Task<UserSkill> CreateUserSkillAsync(UserSkill userskill);
        Task<bool> DeleteUserSkillAsync(int id,string freelancerId);

        Task<List<UserSkill>> GetUserSkillByUserIdAsync(string userId);
    }
}
