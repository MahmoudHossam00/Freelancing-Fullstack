
namespace Freelancing.RepositoryService
{
	public class SubscriptionPlanService(ApplicationDbContext _context) : ISubscriptionPlanService
	{
		public async Task<bool> AddSubscriptionPlan(SubscriptionPlan SubscriptionPlan)
		{
			await _context.SubscriptionPlans.AddAsync(SubscriptionPlan);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> DeleteSubscriptionPlan(int id)
		{
			var plan=await _context.SubscriptionPlans.FirstOrDefaultAsync(p=>p.Id==id);
			if (plan == null)
			{
				return false;
			}
			else
			{
				plan.isDeleted = true;
				_context.SubscriptionPlans.Update(plan);
				return await _context.SaveChangesAsync() > 0;
			}
		}

		public async Task<IEnumerable<SubscriptionPlan>> GetAllSubscriptionPlans()
		{
			return await _context.SubscriptionPlans.ToListAsync();
		}

		public async Task<SubscriptionPlan> GetSubscriptionPlanById(int id)
		{
			return await _context.SubscriptionPlans.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<bool> UpdateSubscriptionPlan(SubscriptionPlan SubscriptionPlan)
		{
			var exp = await _context.SubscriptionPlans.FirstOrDefaultAsync(c => c.Id == SubscriptionPlan.Id);
			if (exp == null)
			{
				return false;
			}
			_context.SubscriptionPlans.Update(SubscriptionPlan);
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
