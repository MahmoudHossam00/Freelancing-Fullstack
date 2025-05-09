using Freelancing.DTOs;

namespace Freelancing.RepositoryService
{
    public class PaymentService(ApplicationDbContext _context) : IPaymentService
    {
        public async Task<List<PaymentDTO>> GetUserPaymentsAsync(string userId)
        {
            var withdrawals = await _context.Withdrawals.Where(p => p.FreelancerId == userId || p.ClientId == userId).ToListAsync();

            var funds = await _context.Funds.Where(p => p.FreelancerId == userId || p.ClientId == userId).ToListAsync();

            var subscriptionPayment = await _context.SubscriptionPayments.Include(p => p.payments).Where(p => p.payments.UserId == userId).ToListAsync();

            var propsalConfirmationPayment = await _context.ClientProposalPayments.Include(p => p.Proposal).Where(p => p.Proposal.Project.ClientId == userId).ToListAsync();

            var allPayments = withdrawals.Cast<Payment>().Concat(funds).Concat(subscriptionPayment).Concat(propsalConfirmationPayment).ToList();

            return allPayments.Select(p => new PaymentDTO
            {
                Amount = p.Amount,
                Date = p.Date,
                PaymentMethod = p.PaymentMethod,
                TransactionId = FormatTransactionId(p.TransactionId, p.PaymentMethod),
                // TransactionId = p.TransactionId,
                TransactionType = p is Withdrawal ? "Withdrawal" : p is AddingFunds ? "AddFunds" : p is SubscriptionPayment ? "SubscriptionPayment" : "PropsalConfirmationPayment"
            }).ToList();




        }
        private string FormatTransactionId(string transactionId, PaymentMethod method)
        {
            if (method == PaymentMethod.CreditCard)
            {

                var cardInfo = transactionId.Split(',');
                if (cardInfo.Length > 0)
                {
                    var cardNumber = cardInfo[0];
                    if (cardNumber.Length >= 4)
                        return "**** **** **** " + cardNumber[^4..];
                }
                return "****";
            }


            return transactionId;
        }

    }
}
