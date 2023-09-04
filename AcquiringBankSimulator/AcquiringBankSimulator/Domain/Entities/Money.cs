namespace AcquiringBankSimulator.Domain.Entities
{
    public record Money
    {
        public Money(decimal amount, string currency)
        {
            Amount=amount; Currency=currency;
        }

        public Money()
        {

        }
        public decimal Amount { get; }
        public string Currency { get; }
    }
}
