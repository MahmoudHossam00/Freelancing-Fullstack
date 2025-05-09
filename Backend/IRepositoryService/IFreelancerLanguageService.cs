using Microsoft.AspNetCore.Mvc;

namespace Freelancing.IRepositoryService
{
    public interface IFreelancerLanguageService
    {
        Task<IEnumerable<FreelancerLanguage>> GetAllLanguagesAsync();
        Task<IEnumerable<FreelancerLanguage>> GetLanguagesByFreelancerUserNameAsync(string username);
        Task<FreelancerLanguage> GetLanguageById(int id);
        Task<bool> UpdateLanguageAsync(FreelancerLanguage language);
        Task<bool> CreateLanguageAsync(FreelancerLanguage language);
        Task<bool> DeleteLanguageAsync(int id);
    }
}
