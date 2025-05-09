
using Freelancing.Models;

namespace Freelancing.RepositoryService
{
	public class ExperienceService(ApplicationDbContext _context) : IExperienceService
	{
		public async Task<bool> AddExperience(Experience Experience)
		{
			await _context.Experiences.AddAsync(Experience);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> DeleteExperience(int id)
		{
			var exp = await GetExperienceById(id);
			if (exp == null)
			{
				return false;
			}
			exp.isDeleted = true;
			_context.Experiences.Update(exp);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> ExperienceExists(int id)
		{
			return await _context.Experiences.AnyAsync(c => c.Id == id);
		}

		public async Task<IEnumerable<Experience>> GetAllExperiences()
		{
			var exp = await _context.Experiences
                //.Include(e => e.Freelancer)                
                .OrderByDescending(e => e.StartDate)
				.ToListAsync();			
                return exp;            
		}

		public async Task<IEnumerable<Experience>> GetExperienceByCompanyName(string name)
		{
            var exp = await _context.Experiences
                .Include(e => e.Freelancer)
                .Where(e => e.Company.Contains(name.Trim()))
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();
            return exp;            
		}

		public async Task<IEnumerable<Experience>> GetExperienceByFreelancerUserName(string username)
		{
            var exp = await _context.Experiences
                .Include(e => e.Freelancer)
				.Where(e => e.Freelancer.UserName == username&&!e.isDeleted)
				.OrderByDescending(e => e.StartDate)
                .ToListAsync();
            return exp;            
		}

		public async Task<Experience> GetExperienceById(int id)
		{
			var exp = await _context.Experiences
				.Include(e => e.Freelancer)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == id);
			return exp;
		}

		public async Task<bool> UpdateExperience(Experience Experience)
		{
			var exp = await GetExperienceById(Experience.Id);
			if (exp == null)
			{
				return false;
			}
			_context.Experiences.Update(Experience);
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
