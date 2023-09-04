namespace PaymentGateway.Application.Infrastructure
{
    public interface IHttpProcessor
    {
        Task<TOutput> SendRequest<TInput, TOutput>(TInput input, HttpMethod method, string url);

    }
}