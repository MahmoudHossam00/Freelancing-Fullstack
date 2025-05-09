
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
	public class SubscriptionPaymentService(ApplicationDbContext _context) : ISubscriptionPaymentService
	{
		public async Task<bool> AddSubscriptionPayment(SubscriptionPayment SubscriptionPayment)
		{
			await _context.SubscriptionPayments.AddAsync(SubscriptionPayment);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> DeleteSubscriptionPayment(int id)
		{
			var payment = await _context.SubscriptionPayments.FirstOrDefaultAsync(p => p.Id == id);
			if (payment == null)
			{
				return false;
			}
			else
			{
				payment.IsDeleted = true;
				_context.SubscriptionPayments.Update(payment);
				return await _context.SaveChangesAsync() > 0;
			}
		}

		public async Task<IEnumerable<SubscriptionPayment>> GetAllSubscriptionPayments()
		{
			return await _context.SubscriptionPayments.ToListAsync();
		}

		public async Task<IEnumerable<SubscriptionPayment>> GetAllSubscriptionPaymentsByPaymentmethod(PaymentMethod method)
		{
			return await _context.SubscriptionPayments
				.Where(p => p.PaymentMethod == method)
				.ToListAsync();
		}

		public async Task<SubscriptionPayment> GetSubscriptionPaymentById(int id)
		{
			return await _context.SubscriptionPayments.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<bool> UpdateSubscriptionPayment(SubscriptionPayment SubscriptionPayment)
		{
			var payment = await _context.SubscriptionPayments.FirstOrDefaultAsync(p => p.Id == SubscriptionPayment.Id);
			if (payment == null)
			{
				return false;
			}
				//payment.isDeleted = true;
				_context.SubscriptionPayments.Update(SubscriptionPayment);
				return await _context.SaveChangesAsync() > 0;
			
		}
	}
}
