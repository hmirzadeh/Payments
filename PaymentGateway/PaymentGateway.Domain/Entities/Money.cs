namespace PaymentGateway.Domain.Entities
{
    public record Money
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
