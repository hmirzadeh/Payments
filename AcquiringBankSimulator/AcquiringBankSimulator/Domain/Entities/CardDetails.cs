using System.Text.Json.Serialization;

namespace AcquiringBankSimulator.Domain.Entities
{
    public record CardDetails
    {
        public CardDetails(string fullName, string cvv, string cardNumber, int expiryYear,int expiryMonth)
        {
            FullName=fullName;
            CardNumber = cardNumber;
            ExpiryYear=expiryYear;
            ExpiryMonth=expiryMonth;
            CVV=cvv;
        }

        public CardDetails()
        {

        }
        public string FullName { get; set; }
        public string CVV { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }

    }
}
