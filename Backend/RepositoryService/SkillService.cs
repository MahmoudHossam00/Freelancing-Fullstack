using Freelancing.IRepositoryService;
//using Freelancing.Migrations;
using Freelancing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class SkillService(ApplicationDbContext context) : ISkillService
    {
        public async Task<Skill> CreateSkillAsync(Skill skill)
        {
            Skill s = new Skill()
            { 
                Id = skill.Id,
                Name = skill.Name
            };
            context.Add(s);
            context.SaveChanges();
            return s;
        }

        public async Task<bool> DeleteSkillAsync(int id)
        {
            var skill = await context.Skills.FirstOrDefaultAsync(x => x.Id == id);
            if (skill == null) 
            { 
                return false;
            }
            skill.IsDeleted = true;
            await context.SaveChangesAsync();
            return true;
            
        }

        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            return await context.Skills.Where(s=>!s.IsDeleted).ToListAsync();
        }

        public async Task<Skill> GetSkillByIDAsync(int skillId)
        {
            var skill = await context.Skills.FirstOrDefaultAsync(s =>s.Id==skillId && !s.IsDeleted);
            return skill;
        }

        
       
        public async Task<Skill> UpdateSkillAsync(Skill skill)
        {
            var existingSkill = await context.Skills.FirstOrDefaultAsync(s => s.Id == skill.Id && !s.IsDeleted);
            if (existingSkill == null)
            {
                return null;
            }
            existingSkill.Name = skill.Name;
            await context.SaveChangesAsync();
            return existingSkill;
        }

        [HttpGet("{id}")]
        public async Task<Skill> GetSkillById(int id)
        {
            var skill = await context.Skills.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (skill == null)
            {
                return null;
            }
            return skill;
        }
    }
}
