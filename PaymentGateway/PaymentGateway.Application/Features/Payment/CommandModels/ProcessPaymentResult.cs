using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.Features.Payment.CommandModels
{
    public record ProcessPaymentResult
    {
        public Guid PaymentId { get; set; }
        public PaymentResult ResultCode { get; set; }
        public string? Result { get; set; }
    }
}
