using Freelancing.DTOs;

namespace Freelancing.IRepositoryService
{
    public interface IPaymentService
    {
        Task<List<PaymentDTO>> GetUserPaymentsAsync(string userId);
       
    }
}
