using Freelancing.DTOs;
using Freelancing.Models;

namespace Freelancing.IRepositoryService
{
    public interface IMilestonePaymentService
    {
        Task<List<MilestonePayment>> GetAllAync();

        Task<MilestonePayment> GetById(int id);

        Task<MilestonePayment> GetByMilestoneId(int id);

        Task<MilestonePayment> AddAsync(MileStonePaymentDTO milestonePayment);

        Task<bool> DeleteAsync(int id);
    }
}
