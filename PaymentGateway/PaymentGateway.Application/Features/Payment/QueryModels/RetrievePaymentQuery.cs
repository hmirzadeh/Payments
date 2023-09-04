namespace PaymentGateway.Application.Features.Payment.QueryModels;

public record RetrievePaymentQuery
{
    public Guid Id { get; set; }
}