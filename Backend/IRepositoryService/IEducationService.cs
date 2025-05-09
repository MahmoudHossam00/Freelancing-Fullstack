using Freelancing.Models;

namespace Freelancing.IRepositoryService;
public interface IEducationService
{
    Task<bool> AddEducation(Education education);
    Task<bool> UpdateEducation(Education education);
    Task<bool> DeleteEducation(int id);
    Task<Education> GetEducationById(int id);
    Task<IEnumerable<Education>> GetAllEducations();
    Task<IEnumerable<Education>> GetAllEducationsByFreelancerUserName(string username);
    Task<bool> EducationExists(int id);
}



