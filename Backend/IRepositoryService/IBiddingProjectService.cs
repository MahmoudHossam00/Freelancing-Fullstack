using Freelancing.DTOs.AuthDTOs;
using Freelancing.DTOs.BiddingProjectDTOs;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IBiddingProjectService
    {
        Task<List<BiddingProjectGetAllDTO>> GetAllBiddingProjectsAsync(BiddingProjectFilterDTO biddingProjectFilters, int pageNumber, int PageSize);
        Task<List<BiddingProjectGetAllDTO>> GetAllBiddingProjectsAsyncByfreelancerId(string id, userRole role);
        Task<List<BiddingProjectGetAllDTO>> GetAllBiddingProjectsAsyncByclientId(string id,BiddingProjectFilterDTO biddingProjectFilters);
        Task<BiddingProjectGetByIdDTO> GetBiddingProjectByIdAsync(int id, string userId);
        Task<BiddingProject> CreateBiddingProjectAsync(BiddingProjectCreateUpdateDTO project, string ClinetId);
        Task<BiddingProject> UpdateBiddingProjectAsync(BiddingProjectCreateUpdateDTO project, int projectDTOid);
        Task<bool> DeleteBiddingProjectAsync(int id);
		Task<List<BiddingProjectGetAllDTO>> GetmyBiddingProjectsAsync(string userId,userRole role,int pageNumber, int PageSize);

		//Task<List<BiddingProjectGetAllDTO>> Filter(BiddingProjectFilterDTO biddingProjectFilters, int pageNumber, int PageSize);

	}
}
