using System.Text.Json.Serialization;

namespace AcquiringBankSimulator.Domain.Entities
{
    public record CardExpiry
    {

        public CardExpiry(int year, int month)
        {
            ExpiryYear = year;
            ExpiryMonth = month;
        }
        [JsonPropertyName("expiryMonth")]
        public int ExpiryMonth { get; }
        [JsonPropertyName("expiryYear")]
        public int ExpiryYear { get; }
    }
}
