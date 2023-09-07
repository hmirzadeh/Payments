using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Api.Payments;
using PaymentGateway.Application.Features.Payment.CommandModels;
using PaymentGateway.Application.Features.Payment.QueryModels;
using PaymentGateway.Application.Features.Payment.Services;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Api.UnitTests.Controllers
{
    public class PaymentGatewayControllerTests
    {
        Mock<IPaymentService> _paymentServiceMock;
        ProcessPaymentResult _submitPaymentResult;
        private ProcessPaymentCommand _paymentCommand;
        private RetrievePaymentResult _paymentResult;
        [SetUp]
        public void Setup()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _paymentCommand = new ProcessPaymentCommand()
            {
                CardNumber = "1234567890121233",
                Amount = 15.5m,
                ExpiryYear = DateTime.Now.Year + 5,
                ExpiryMonth = 12,
                Currency = "GBP",
                CVV = "123",
                FullName = "Test name",
                MerchantId = "123456"

            };


            _paymentResult = new RetrievePaymentResult()
            {
                CardNumber = "1234567890121233",
                Result = "Successful",
                Amount = 15.5m,
                ExpiryYear = DateTime.Now.Year + 5,
                ExpiryMonth = 12,
                Currency = "GBP",
                CreateDateTime = DateTime.Now.AddDays(-5),
                Id = Guid.NewGuid(),
            };

        }

        [Test]
        public void Given_POST_Endpoint_When_Payment_Is_Valid_Then_Response_Should_Be_Ok_200()
        {
            _submitPaymentResult = new ProcessPaymentResult()
            {
                ResultCode = PaymentResult.Successful,
                Result = "Successful",
                PaymentId = Guid.NewGuid()
            };

            _paymentServiceMock.Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentCommand>())).ReturnsAsync(_submitPaymentResult);
            var paymentGatewayController = new PaymentGatewayController(_paymentServiceMock.Object);
            var response = paymentGatewayController.Post(_paymentCommand).Result as ObjectResult; ;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status200OK);
        }

        [Test]
        public void Given_POST_Endpoint_When_ProcessPayment_Service_Has_Error_Then_Response_Should_Be_InternalError_500()
        {
            _submitPaymentResult = new ProcessPaymentResult()
            {
                ResultCode = PaymentResult.InternalError,
                Result = "InternalError",
                PaymentId = Guid.NewGuid()
            };

            _paymentServiceMock.Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentCommand>())).ReturnsAsync(_submitPaymentResult);
            var paymentGatewayController = new PaymentGatewayController(_paymentServiceMock.Object);
            var response = paymentGatewayController.Post(_paymentCommand).Result as ObjectResult; ;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status500InternalServerError);
        }

        [Test]
        public void Given_GET_Endpoint_Triggered_When_PaymentId_Is_Valid_Then_It_Should_Return_200_And_Payment()
        {
            _paymentServiceMock.Setup(x => x.RetrievePayment(It.IsAny<string>())).ReturnsAsync(_paymentResult);
            var paymentGatewayController = new PaymentGatewayController(_paymentServiceMock.Object);
            var response = paymentGatewayController.Get("Guid-string-1234").Result as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status200OK);
            Assert.AreEqual(response.Value, _paymentResult);

        }

        [Test]
        public void Given_GET_Endpoint_Triggered_When_RetrievePayment_Throws_Error_Then_It_Should_Return_500_InternalError()
        {
            _paymentServiceMock.Setup(x => x.RetrievePayment(It.IsAny<string>())).ThrowsAsync(new Exception());
            var paymentGatewayController = new PaymentGatewayController(_paymentServiceMock.Object);
            var response = paymentGatewayController.Get("Guid-string-1234").Result as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, StatusCodes.Status500InternalServerError);

        }
        [Test]
        public void Given_GET_Endpoint_Triggered_When_Payment_doesnt_Exist_Then_It_Should_Return_404()
        {
            _paymentServiceMock.Setup(x => x.RetrievePayment(It.IsAny<string>())).ReturnsAsync(new RetrievePaymentResult());
            var paymentGatewayController = new PaymentGatewayController(_paymentServiceMock.Object);
            var response = paymentGatewayController.Get("Guid-string-1234").Result as ObjectResult;
            Assert.AreEqual(response.StatusCode, StatusCodes.Status404NotFound);

        }
    }
}