namespace PaymentGateway.Domain.Entities
{
    public record CardDetails
    {
        public string FullName { get; set; }
        public string CVV { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }

    }
}
