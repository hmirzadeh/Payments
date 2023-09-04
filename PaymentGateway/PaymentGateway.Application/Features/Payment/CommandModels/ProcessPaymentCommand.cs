namespace PaymentGateway.Application.Features.Payment.CommandModels
{
    public record ProcessPaymentCommand
    {
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public string MerchantId { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
}
