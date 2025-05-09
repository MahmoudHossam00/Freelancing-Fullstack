
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
	public class UserSubscriptionPlanPaymentService(ApplicationDbContext _context) : IUserSubscriptionPlanPaymentService
	{
		public async Task<bool> AddUserSubscriptionPlanPayment(UserSubscriptionPlanPayment UserSubscriptionPlanPayment)
		{
			await _context.UserSubscriptionPlanPayments.AddAsync(UserSubscriptionPlanPayment);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> DeleteUserSubscriptionPlanPayment(int id)
		{
			var plan = await _context.UserSubscriptionPlanPayments.FirstOrDefaultAsync(p => p.Id == id);
			if (plan == null)
			{
				return false;
			}
			else
			{
				plan.isDeleted = true;
				_context.UserSubscriptionPlanPayments.Update(plan);
				return await _context.SaveChangesAsync() > 0;
			}
		}
		public async Task<bool> UpdateUserSubscriptionPlanPayment(UserSubscriptionPlanPayment UserSubscriptionPlanPayment)
		{
			var plan = await _context.UserSubscriptionPlanPayments.FirstOrDefaultAsync(p => p.Id == UserSubscriptionPlanPayment.Id);
			if (plan == null)
			{
				return false;
			}
			else
			{
				_context.UserSubscriptionPlanPayments.Update(UserSubscriptionPlanPayment);
				return await _context.SaveChangesAsync() > 0;
			}
		}

		public async Task<IEnumerable<UserSubscriptionPlanPayment>> GetAllUserSubscriptionPlanPayments()
		{
			return await _context.UserSubscriptionPlanPayments.ToListAsync();
		}

		public async Task<UserSubscriptionPlanPayment> GetUserSubscriptionPlanPaymentById(int id)
		{
			return await _context.UserSubscriptionPlanPayments.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<IEnumerable<UserSubscriptionPlanPayment>> GetUserSubscriptionPlanPaymentsBySubscriptionId(int id)
		{
			return await _context.UserSubscriptionPlanPayments.Where(p => p.SubscriptionPlanId == id).ToListAsync();
		}

		public async Task<IEnumerable<UserSubscriptionPlanPayment>> GetUserSubscriptionPlanPaymentsByUserId(string id)
		{
			return await _context.UserSubscriptionPlanPayments.Where(p => p.UserId == id).ToListAsync();
		}

		
	}
}
