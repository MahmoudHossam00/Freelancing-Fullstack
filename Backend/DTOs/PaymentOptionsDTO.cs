namespace Freelancing.DTOs
{
    public class PaymentOptionsDTO
    {
        public List<PaymentMethodDTO> DepositMethods { get; set; }
        public List<PaymentMethodDTO> WithdrawMethods { get; set; }
    }
}
