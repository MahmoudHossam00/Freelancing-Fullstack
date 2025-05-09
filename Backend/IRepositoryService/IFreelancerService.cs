using Freelancing.DTOs.AuthDTOs;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IFreelancerService
    {
        Task<List<ViewFreelancersDTO>> GetAllFiltered(FreelancerFilterationDTO dto);
		Task<List<ViewFreelancersDTO>> GetAllAsync();
        Task<ViewFreelancerPageDTO> GetByIDAsync(string id);
		Task<Freelancer> UpdateFreelancerAsync(Freelancer freelancer);
        Task<Freelancer> CreateFreelancerAsync(Freelancer freelancer);
        Task<bool> DeleteFreelancerAsync(string id);


    }
}
