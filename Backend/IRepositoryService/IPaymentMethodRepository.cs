namespace Freelancing.IRepositoryService
{
    public interface IPaymentMethodRepository
    {
        Task<List<PaymentMethod>> GetDepositMethodsAsync();
        Task<List<PaymentMethod>> GetWithdrawMethodsAsync();
    }
}
