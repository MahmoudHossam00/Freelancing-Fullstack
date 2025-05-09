namespace Freelancing.RepositoryService
{
    public class ProjectRepository:IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllWithSkillsAsync()
        {
            return await _context.project
                .Include(p => p.ProjectSkills)
                    .ThenInclude(ps => ps.Skill)
                .ToListAsync();
        }
    }
}
