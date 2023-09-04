using AcquiringBankSimulator.Domain.Enum;

namespace AcquiringBankSimulator.Domain.Entities
{
    public record Payment
    {
        public string MerchantId { get; set; }
        public CardDetails Card { get; set; }
        public PaymentResult Result { get; set; }
        public Money Money { get; set; }
    }
}
