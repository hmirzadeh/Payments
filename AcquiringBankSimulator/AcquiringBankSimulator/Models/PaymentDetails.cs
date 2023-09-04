namespace AcquiringBankSimulator.Models
{
    public class PaymentDetails
    {
        public long CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int CVV { get; set; }

    }
}
