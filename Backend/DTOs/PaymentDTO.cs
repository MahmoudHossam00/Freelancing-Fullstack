namespace Freelancing.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string TransactionId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string TransactionType { get; set; }

        public string UserId { get; set; }  
    }
}
