using Freelancing.DTOs;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IPortofolioProject
    {
        Task<List<PortofolioProject>> GetAllAync();

        Task<PortofolioProject> GetByIdAsync(int id);

        Task<List<PortofolioProject>> GetByFreelancerId(string id);

        Task<PortofolioProject> AddAsync(CreatePortfolioProjectDTO portofolioProject, string freelancerid);


		Task<PortofolioProject> UpdateAsync(PortofolioProjectDTO portofolioProject);

        Task<bool> DeleteAsync(int id);
    }
}
