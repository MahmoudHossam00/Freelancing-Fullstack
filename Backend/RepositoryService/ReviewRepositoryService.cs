using AutoMapper;
using Freelancing.DTOs;
using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class ReviewRepositoryService : IReviewRepositoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewRepositoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Reviewee)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(r => r.id == id);
        }

        //public async Task<List<Review>> GetReviewsByRevieweeIdAsync(string revieweeId)
        //{
        //    return await _context.Reviews
        //        .Include(r => r.Reviewee)
        //        .Include(r => r.Reviewer)
        //        .Where(r => r.RevieweeId == revieweeId)
        //        .OrderByDescending(r => r.id)
        //        .ToListAsync();
        //}

        public async Task<List<GetReviewByRevieweeIdDto>> GetReviewsByRevieweeIdAsync(string revieweeId)
        {
           var reviews= await _context.Reviews
                .Include(r => r.Reviewee)
                .Include(r => r.Reviewer)
                .Include(r=>r.Project)
                .Where(r => r.RevieweeId == revieweeId)
                .OrderByDescending(r => r.id)
                .ToListAsync();
            
            var ReviewsDto = new List<GetReviewByRevieweeIdDto>();
            foreach(var review in reviews)
            {
                var addedreview = _mapper.Map<GetReviewByRevieweeIdDto>(review);
				addedreview.ReviewerId = review.ReviewerId;
				ReviewsDto.Add(addedreview);
			}
            return ReviewsDto;
        }

        public async Task<List<Review>> GetReviewsByReviewerIdAsync(string reviewerId)
        {
            return await _context.Reviews
                .Include(r => r.Reviewee)
                .Include(r => r.Reviewer)
                .Where(r => r.ReviewerId == reviewerId)
                .OrderByDescending(r => r.id)
                .ToListAsync();
        }
		public async Task<List<Review>> GetReviewsByProjectIdAsync(int projectid)
		{
			return await _context.Reviews
				.Include(r => r.Reviewee)
				.Include(r => r.Reviewer)
				.Where(r => r.ProjectId == projectid)
				.OrderByDescending(r => r.id)
				.ToListAsync();
		}

		public async Task<Review> CreateReviewAsync(Review review)
        {

           var project= _context.project.FirstOrDefault(p => p.Id == review.ProjectId);
            if(project==null)
            {
                return null;
            }
            if ((review.RevieweeId == project.FreelancerId && review.ReviewerId == project.ClientId) ||
                (review.RevieweeId == project.ClientId && review.ReviewerId == project.FreelancerId))
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return review;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }



    }
}