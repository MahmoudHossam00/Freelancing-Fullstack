namespace Freelancing.IRepositoryService
{
	public interface ISubscriptionPaymentService
	{
		Task<bool> AddSubscriptionPayment(SubscriptionPayment SubscriptionPayment);
		Task<bool> UpdateSubscriptionPayment(SubscriptionPayment SubscriptionPayment);
		Task<bool> DeleteSubscriptionPayment(int id);
		Task<SubscriptionPayment> GetSubscriptionPaymentById(int id);
		Task<IEnumerable<SubscriptionPayment>> GetAllSubscriptionPayments();
		Task<IEnumerable<SubscriptionPayment>> GetAllSubscriptionPaymentsByPaymentmethod(PaymentMethod method);
	}
}
