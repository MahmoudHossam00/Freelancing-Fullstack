using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;

namespace Freelancing.RepositoryService
{
    public class FreelancerLanguageService(ApplicationDbContext _context) : IFreelancerLanguageService
    {
        public async Task<bool> CreateLanguageAsync(FreelancerLanguage language)
        {
            await _context.freelancerLanguages.AddAsync(language);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<bool> DeleteLanguageAsync(int id)
        {
            var language = await GetLanguageById(id);
            if (language == null)
            {
                return false;
            }
            language.IsDeleted = true;
            _context.Update(language);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<FreelancerLanguage>> GetAllLanguagesAsync()
        {
            return await _context.freelancerLanguages
                .Include(c => c.freelancer)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<FreelancerLanguage> GetLanguageById(int id)
        {
            var lang = await _context.freelancerLanguages
                .Include(c => c.freelancer)
                .FirstOrDefaultAsync(c => c.id == id && !c.IsDeleted);
            return lang;
        }

        public async Task<IEnumerable<FreelancerLanguage>> GetLanguagesByFreelancerUserNameAsync(string username)
        {
            var languages = await _context.freelancerLanguages
                .Include(c => c.freelancer)
                .Where(c => c.freelancer.UserName == username&& !c.IsDeleted)
                .ToListAsync();
            return languages;
        }

        public async Task<bool> UpdateLanguageAsync(FreelancerLanguage language)
        {
            var existinglanguage = await GetLanguageById(language.id);
            if (existinglanguage == null)
            {
                return false;
            }
            _context.freelancerLanguages.Update(language);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
