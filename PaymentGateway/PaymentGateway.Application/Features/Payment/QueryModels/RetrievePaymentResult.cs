namespace PaymentGateway.Application.Features.Payment.QueryModels;

public record RetrievePaymentResult
{
    public Guid Id { get; set; }
    public string Result { get; set; }
    public string CardNumber { get; set; }
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public int ExpiryMonth { get; set; }
    public DateTime CreateDateTime { get; set; }
    public int ExpiryYear { get; set; }
}