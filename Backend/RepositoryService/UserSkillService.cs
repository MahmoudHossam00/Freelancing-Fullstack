using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class UserSkillService(ApplicationDbContext context) : IUserSkillService
    {
        public async Task<UserSkill> CreateUserSkillAsync(UserSkill userskill)
        {
            UserSkill NewUserSkill = new UserSkill()
            {
                FreelancerId = userskill.FreelancerId,
                SkillId = userskill.SkillId,
               
                IsDelete = false,

            };
            context.UserSkills.Add(NewUserSkill);

            await context.SaveChangesAsync();

            return await GetUserSkillByIDAsync(NewUserSkill.id);
        }

        public async Task<bool> DeleteUserSkillAsync(int id, string freelancerId)
        {
            var userSkill = await context.UserSkills.FirstOrDefaultAsync(u => u.id == id && u.FreelancerId == freelancerId && !u.IsDelete);
            if (userSkill == null) 
            { 
                return false;
            }
            userSkill.IsDelete = true; 
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserSkill>> GetAllUserSkillAsync()
        {
           return await context.UserSkills.Include(s =>s.Skill).Where(u=>!u.IsDelete).ToListAsync();
        }

        public async Task<UserSkill> GetUserSkillByIDAsync(int id)
        {
            return await context.UserSkills.Include(s=> s.Skill).FirstOrDefaultAsync(u => u.id == id && !u.IsDelete);
        }

     

        public async Task<UserSkill> UpdateUserSkillAsync(UserSkill userskill, string freelancerId)
        {
            var existingUserSkill = await context.UserSkills.Include(s=>s.Skill).FirstOrDefaultAsync(u => u.id == userskill.id && u.FreelancerId == freelancerId && !u.IsDelete);
            if (existingUserSkill == null)
            {
                return null;
            }
            //existingUserSkill.FreelancerId = userskill.FreelancerId;
            existingUserSkill.SkillId = userskill.SkillId;
            if (existingUserSkill == null)
            {
                throw new Exception("Skill not found or does not belong to current freelancer.");
            }

            await context.SaveChangesAsync();
            return existingUserSkill;

        }

        public async Task<List<UserSkill>> GetUserSkillByUserIdAsync(string userId)
        {
            return await context.UserSkills.Include(s =>s.Skill).Where(u => u.FreelancerId == userId && !u.IsDelete).ToListAsync();
        }
    }
}
