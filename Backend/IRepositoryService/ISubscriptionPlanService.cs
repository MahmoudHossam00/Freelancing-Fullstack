namespace Freelancing.IRepositoryService
{
	public interface ISubscriptionPlanService
	{
		Task<bool> AddSubscriptionPlan(SubscriptionPlan SubscriptionPlan);
		Task<bool> UpdateSubscriptionPlan(SubscriptionPlan SubscriptionPlan);
		Task<bool> DeleteSubscriptionPlan(int id);
		Task<SubscriptionPlan> GetSubscriptionPlanById(int id);
		Task<IEnumerable<SubscriptionPlan>> GetAllSubscriptionPlans();
	}
}
