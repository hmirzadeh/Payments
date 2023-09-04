using System.Text.Json.Serialization;

namespace AcquiringBankSimulator.Domain.Entities
{
    public class PaymentDetails
    {
        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; }

        [JsonPropertyName("expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("cvv")]
        public string CVV { get; set; }

    }
}
