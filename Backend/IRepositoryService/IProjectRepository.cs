namespace Freelancing.IRepositoryService
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllWithSkillsAsync();
    }
}
