namespace AcquiringBankSimulator.Domain.Entities
{
    public record CompletedPayment: Payment
    {
        public Guid Id { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
