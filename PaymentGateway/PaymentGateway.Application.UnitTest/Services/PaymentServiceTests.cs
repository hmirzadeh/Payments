using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using PaymentGateway.Application.Features.Payment.CommandModels;
using PaymentGateway.Application.Features.Payment.QueryModels;
using PaymentGateway.Application.Features.Payment.Services;
using PaymentGateway.Application.Infrastructure;
using PaymentGateway.Domain.Configurations;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.UnitTests.Services
{
    public class PaymentServiceTests
    {
        private CompletedPayment _completedResult;
        private Mock<IHttpProcessor> _mockHttpProcessor;
        private IOptions<AcquiringBank> options;
        private PaymentService _service;
        private Mock<IMapper> _mapper;
        private RetrievePaymentResult _paymentResult;
        private ProcessPaymentCommand _paymentCommand;
        private Payment _payment;
        [SetUp]
        public void Setup()
        {
            _mockHttpProcessor = new Mock<IHttpProcessor>();
            _mapper = new Mock<IMapper>();
            var card = new CardDetails()
            {
                CardNumber = "1234567890121233",
                ExpiryMonth = 12,
                ExpiryYear = DateTime.Now.Year + 5,
                CVV = "123",
                FullName = "John Smith"

            };

            var money = new Money()
            {
                Amount = 15.5m,
                Currency = "GBP"
            };
            _completedResult = new CompletedPayment()
            {
                Card = card,
                CreateDateTime = DateTime.Now.AddDays(-5),
                Id = Guid.NewGuid(),
                MerchantId = "12345678",
                Money = money,
                Result = PaymentResult.Successful

            };
            _payment = new Payment()
            {
                Card = card,
                MerchantId = "12345678",
                Money = money,
                Result = PaymentResult.Successful,
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

            options = Options.Create(new AcquiringBank() { BaseURL = "someURL" });

        }

        [Test]
        public void Given_ProcessPayment_When_Payment_Is_Valid_Then_PaymentResult_Should_Be_Successful_And_PaymentId_IsNot_Null()
        {
            _mockHttpProcessor.Setup(x => x.SendRequest<Payment, PaymentResult>(
                It.IsAny<Payment>(), It.IsAny<HttpMethod>(), It.IsAny<string>())).ReturnsAsync(PaymentResult.Successful);

            _mapper.Setup(x => x.Map<Payment>(It.IsAny<ProcessPaymentCommand>())).Returns(_payment);

            _service = new PaymentService(_mockHttpProcessor.Object, options, _mapper.Object);
            var payment = _service.ProcessPayment(_paymentCommand).Result;

            Assert.IsNotNull(payment);
            Assert.IsInstanceOf(typeof(ProcessPaymentResult), payment);
            Assert.That(payment.ResultCode, Is.EqualTo(PaymentResult.Successful));
            Assert.That(payment.Result, Is.EqualTo("Successful"));
            Assert.IsNotNull(payment.PaymentId);

        }
        [Test]
        public void Given_ProcessPayment_When_Infrastructure_Layer_Throws_Exception_Then_PaymentResult_Should_Be_InternalError()
        {
            _mockHttpProcessor.Setup(x => x.SendRequest<Payment, PaymentResult>(
                It.IsAny<Payment>(), It.IsAny<HttpMethod>(), It.IsAny<string>())).Throws<Exception>();

            _mapper.Setup(x => x.Map<Payment>(It.IsAny<ProcessPaymentCommand>())).Returns(_payment);

            _service = new PaymentService(_mockHttpProcessor.Object, options, _mapper.Object);
            var payment = _service.ProcessPayment(_paymentCommand).Result;

            Assert.IsNotNull(payment);
            Assert.IsInstanceOf(typeof(ProcessPaymentResult), payment);
            Assert.That(payment.ResultCode, Is.EqualTo(PaymentResult.InternalError));
            Assert.That(payment.Result, Is.EqualTo("InternalError"));

        }

        [Test]
        public void Given_RetreivePayment_When_PaymentId_Is_Valid_Then_It_Should_Return_A_Payment_And_Mask_CardNumber()
        {
            _mockHttpProcessor.Setup(x => x.SendRequest<It.IsAnyType, CompletedPayment>(
                It.IsAny<It.IsAnyType>(), It.IsAny<HttpMethod>(), It.IsAny<string>())).ReturnsAsync(_completedResult);

            _mapper.Setup(x => x.Map<RetrievePaymentResult>(It.IsAny<CompletedPayment>())).Returns(_paymentResult);

            _service = new PaymentService(_mockHttpProcessor.Object, options, _mapper.Object);
            var payment = _service.RetrievePayment(123455).Result;

            Assert.IsNotNull(payment);
            Assert.IsInstanceOf(typeof(RetrievePaymentResult), payment);
            var countX = payment.CardNumber.Count(c => c == '*');
            Assert.That(countX, Is.EqualTo(payment.CardNumber.Length - 4));

        }
    }
}