namespace PaymentGateway.Domain.Exceptions
{
    public record BaseErrorResponse
    {
        public BaseErrorResponse(int statusCode, string message)
        {
            HttpStatusCode=statusCode;
            Message=message;
        }
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
    }
}
