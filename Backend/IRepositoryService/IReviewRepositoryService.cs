using Freelancing.DTOs;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IReviewRepositoryService
    {
        Task<Review> GetReviewByIdAsync(int id);
        //Task<List<Review>> GetReviewsByRevieweeIdAsync(string revieweeId);
        Task<List<GetReviewByRevieweeIdDto>> GetReviewsByRevieweeIdAsync(string revieweeId);
        Task<List<Review>> GetReviewsByReviewerIdAsync(string reviewerId);
        Task<Review> CreateReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Task<List<Review>> GetReviewsByProjectIdAsync(int projectid);


	}
}
