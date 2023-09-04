using PaymentGateway.Application.Common.Validators;
using PaymentGateway.Application.Features.Payment.CommandModels;

namespace PaymentGateway.Application.UnitTests.Validators
{
    public class PaymentValidatorTests
    {
        private PaymentValidator _validator;
        private ProcessPaymentCommand _paymentCommand;
        [SetUp]
        public void SetUp()
        {
            _validator = new PaymentValidator();
            _paymentCommand = new ProcessPaymentCommand()
            {
                Amount = 10,
                CVV = "121",
                CardNumber = "1234567890123456",
                Currency = "GBP",
                ExpiryMonth = 12,
                ExpiryYear = 23,
                FullName = "Test Name",
                MerchantId = "1234567"

            };
        }

        [Test]
        public void Given_All_Other_Value_Are_Valid_When_Cvv_Is_Not_3_Characters_Then_There_Is_An_Error_For_Cvv_In_Response()
        {
            _paymentCommand.CVV = "1233445";

            var result = _validator.Validate(_paymentCommand);

            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.False(result.IsValid);
            Assert.That(result.Errors.Any(o=>o.PropertyName=="CVV"));
        }

        [Test]
        public void Given_All_Other_Value_Are_Valid_When_CardNumber_Is_Less_Than_15_Characters_Then_There_Is_An_Error_For_CardNumber_In_Response()
        {
            _paymentCommand.CardNumber = "123456";

            var result = _validator.Validate(_paymentCommand);

            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.False(result.IsValid);
            Assert.That(result.Errors.Any(o => o.PropertyName == "CardNumber"));
        }

        [Test]
        public void Given_All_Other_Value_Are_Valid_When_Amount_Is_Zero_Then_There_Is_An_Error_For_Amount_In_Response()
        {

            _paymentCommand.Amount = 0;
            var result = _validator.Validate(_paymentCommand);

            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.False(result.IsValid);
            Assert.That(result.Errors.Any(o => o.PropertyName == "Amount"));
        }
    }
}
