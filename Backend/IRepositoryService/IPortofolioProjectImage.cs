using Freelancing.DTOs;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IPortofolioProjectImage
    {
        Task<List<PortofolioProjectImage>> GetByPortfolioProjectIdAsync(int id);

        Task<PortofolioProjectImage> AddAsync(PortofolioProjectImageDTO portofolioProjectImage);

        Task<bool> DeleteAsync(int id);
    }
}
