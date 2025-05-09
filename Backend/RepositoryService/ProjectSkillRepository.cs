namespace Freelancing.RepositoryService
{
    public class ProjectSkillRepository : IProjectSkillRepository
    {

   
            private readonly ApplicationDbContext _context;

            public ProjectSkillRepository(ApplicationDbContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

        public async Task<List<ProjectSkill>> GetAllAsync() {

            return await _context.ProjectSkills
                .Include(ps => ps.Skill)
                .Where(ps => !ps.IsDelete)
                .ToListAsync();
        }

            public async Task<ProjectSkill?> GetByIdAsync(int id)
            {
                return await _context.ProjectSkills
                    .Include(ps => ps.Skill)
                    .FirstOrDefaultAsync(ps => ps.id == id && !ps.IsDelete);
            }

        public async Task<ProjectSkill> CreateProjectSkill(ProjectSkill projectSkill)
        {

            if (projectSkill == null)
                throw new ArgumentNullException(nameof(projectSkill));
            if (!await ProjectExistsAsync(projectSkill.ProjectId))
                throw new InvalidOperationException($"Project with ID {projectSkill.ProjectId} does not exist.");
            if (!await SkillExistsAsync(projectSkill.SkillId))
                throw new InvalidOperationException($"Skill with ID {projectSkill.SkillId} does not exist.");
            _context.ProjectSkills.Add(projectSkill);
            await _context.SaveChangesAsync();
            return projectSkill;

        }

            public async Task<ProjectSkill?> UpdateAsync(ProjectSkill projectSkill)
            {
                var existing = await _context.ProjectSkills
                    .FirstOrDefaultAsync(ps => ps.id == projectSkill.id && !ps.IsDelete);

                if (existing == null)
                    return null;

                if (!await ProjectExistsAsync(projectSkill.ProjectId))
                    throw new InvalidOperationException($"Project with ID {projectSkill.ProjectId} does not exist.");
                if (!await SkillExistsAsync(projectSkill.SkillId))
                    throw new InvalidOperationException($"Skill with ID {projectSkill.SkillId} does not exist.");

                existing.ProjectId = projectSkill.ProjectId;
                existing.SkillId = projectSkill.SkillId;
                await _context.SaveChangesAsync();
                return existing;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var projectSkill = await _context.ProjectSkills
                    .FirstOrDefaultAsync(ps => ps.id == id && !ps.IsDelete);

                if (projectSkill == null)
                    return false;

                projectSkill.IsDelete = true;
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<bool> ExistsAsync(int projectId, int skillId)
            {
                return await _context.ProjectSkills
                    .AnyAsync(ps => ps.ProjectId == projectId && ps.SkillId == skillId && !ps.IsDelete);
            }

            public async Task<bool> ProjectExistsAsync(int projectId)
            {
                return await _context.project.AnyAsync(p => p.Id == projectId && !p.IsDeleted);
            }

            public async Task<bool> SkillExistsAsync(int skillId)
            {
                return await _context.Skills.AnyAsync(s => s.Id == skillId && !s.IsDeleted);
            }

   
    }
    }
