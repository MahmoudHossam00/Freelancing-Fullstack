namespace Freelancing.DTOs
{
    public class MileStonePaymentDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime Date { get; set; }
        public string TransactionId { get; set; }
        public int MilestoneId { get; set; }
    }
}
