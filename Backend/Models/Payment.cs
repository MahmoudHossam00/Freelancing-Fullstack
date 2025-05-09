public abstract class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime Date { get; set; }
    public string TransactionId { get; set; }
    public bool IsDeleted { get; set; } = false;

    //public string? UserId { get; set; }
   // public TransactionType TransactionType { get; set; }
   

}
public enum PaymentMethod
{
	CreditCard,
	Stripe,
    Balance
}

