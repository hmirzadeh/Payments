using PaymentGateway.Application.Features.Payment.CommandModels;
using PaymentGateway.Application.Features.Payment.QueryModels;

namespace PaymentGateway.Application.Features.Payment.Services
{
    public interface IPaymentService
    {
        Task<ProcessPaymentResult> ProcessPayment(ProcessPaymentCommand payment);
        Task<RetrievePaymentResult> RetrievePayment(int paymentQuery);

    }
}
