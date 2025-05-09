namespace Freelancing.IRepositoryService
{
	public interface IUserSubscriptionPlanPaymentService
	{
		Task<bool> AddUserSubscriptionPlanPayment(UserSubscriptionPlanPayment UserSubscriptionPlanPayment);
		Task<bool> UpdateUserSubscriptionPlanPayment(UserSubscriptionPlanPayment UserSubscriptionPlanPayment);
		Task<bool> DeleteUserSubscriptionPlanPayment(int id);
		Task<UserSubscriptionPlanPayment> GetUserSubscriptionPlanPaymentById(int id);
		Task<IEnumerable<UserSubscriptionPlanPayment>> GetUserSubscriptionPlanPaymentsByUserId(string id);
		Task<IEnumerable<UserSubscriptionPlanPayment>> GetUserSubscriptionPlanPaymentsBySubscriptionId(int id);
		Task<IEnumerable<UserSubscriptionPlanPayment>> GetAllUserSubscriptionPlanPayments();
	}
}
