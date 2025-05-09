
namespace Freelancing.Controllers
{
    internal class DepositPayment : Payment
    {
        public object Amount { get; set; }
        public DateTime Date { get; set; }
        public object TransactionId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
     //   public PaymentDirection Direction { get; set; }
        public object UserId { get; set; }
    }
}