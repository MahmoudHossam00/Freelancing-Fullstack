using Freelancing.DTOs;
using Freelancing.IRepositoryService;
//using Freelancing.Migrations;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class MilestonePaymentService(ApplicationDbContext context) : IMilestonePaymentService
    {

        public async Task<List<MilestonePayment>> GetAllAync()
        {
            return await context.MilestonePayments.Where(m=>!m.IsDeleted).ToListAsync();
        }

        public async Task<MilestonePayment> GetById(int id)
        {
            return await context.MilestonePayments.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MilestonePayment> GetByMilestoneId(int id)
        {
            var milestone = context.Milestones.SingleOrDefault(m => m.Id == id);
            if(milestone is not null)
            {
                return await context.MilestonePayments.SingleOrDefaultAsync(m => m.MilestoneId == milestone.Id);
            }
            throw new KeyNotFoundException("Invalid milestone Id");
        }


        public async Task<MilestonePayment> AddAsync(MileStonePaymentDTO milestonePayment)
        {
            MilestonePayment mp = new MilestonePayment()
            {
                Amount=milestonePayment.Amount,
                PaymentMethod=milestonePayment.PaymentMethod,
                Date=milestonePayment.Date,
                TransactionId=milestonePayment.TransactionId,
                MilestoneId = milestonePayment.MilestoneId
            };
            await context.AddAsync(mp);
            await context.SaveChangesAsync();
            return mp;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var milestone = context.MilestonePayments.SingleOrDefault(m => m.Id == id && !m.IsDeleted);
            if(milestone is not null)
            {
                milestone.IsDeleted = true;
                context.MilestonePayments.Update(milestone);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
