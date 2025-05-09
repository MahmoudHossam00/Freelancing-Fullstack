
namespace Freelancing.RepositoryService
{
    public class EducationService(ApplicationDbContext _context) : IEducationService
    {
        public async Task<bool> AddEducation(Education education)
        {
            if(_context.Educations.Any(e=>e.FreelancerId==education.FreelancerId))
            {
                var duplicate = await _context.Educations.FirstOrDefaultAsync(e => e.FreelancerId == education.FreelancerId);
                _context.Educations.Remove(duplicate);
                await _context.SaveChangesAsync();

			}
            await _context.Educations.AddAsync(education);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteEducation(int id)
        {
            var selectededucation = await GetEducationById(id);
            if (selectededucation != null)
            {
                selectededucation.IsDeleted = true;
                _context.Educations.Update(selectededucation);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> EducationExists(int id)
        {
            return await _context.Educations
                .AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Education>> GetAllEducations()
        {
            var educations =await _context.Educations
                .Include(c => c.Freelancer)                
                .ToListAsync();
            if (educations.Count == 0)
            {
                return null;
            }
                return educations;
        }
        public async Task<IEnumerable<Education>> GetAllEducationsByFreelancerUserName(string username)
        {
            var educations =await _context.Educations
                .Include(c => c.Freelancer) 
                .Where(c=>c.Freelancer.UserName == username&&!c.IsDeleted)
                .ToListAsync();
            //if (educations.Count == 0)
            //{
            //    return null;
            //}
                return educations;
        }


        public Task<Education> GetEducationById(int id)
        {
            var education = _context.Educations
                .Include(c => c.Freelancer)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (education == null)
            {
                return null;
            }
            return education;
        }

        public async Task<bool> UpdateEducation(Education education)
        {
            var selectededucation =await GetEducationById(education.Id);
            if (selectededucation != null)
            {
                _context.Educations.Update(education);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
