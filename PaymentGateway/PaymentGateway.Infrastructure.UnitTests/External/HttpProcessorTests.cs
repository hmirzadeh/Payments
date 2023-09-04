using Moq;
using Moq.Protected;
using PaymentGateway.Application.Features.Payment.QueryModels;
using PaymentGateway.Infrastructure.ExternalData;

namespace PaymentGateway.Infrastructure.UnitTests.External
{
    public class HttpProcessorTests
    {
        private Mock<IHttpClientFactory> _mockHttpClient;
        [SetUp]
        public void Setup()
        {
            var jsonString =
                string.Format("{{\"id\": \"c76a958b-74fd-40b2-9048-11a47c4a5152\", \"result\": \"Successful\", \"cardNumber\": \"********9012\", \"currency\": \"GBP\", \"amount\": 15.5, \"expiryMonth\": 11, \"createDateTime\": \"2023-08-29T18:03:38.3859433+01:00\", \"expiryYear\": 2030}}");
            _mockHttpClient = new Mock<IHttpClientFactory>();
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var result = new HttpResponseMessage()
            {
                Content = content
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(result)
                .Verifiable();
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://SomeBaseURL.com")
            };


            _mockHttpClient.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        }

        [Test]
        public void Given_SendAsync_Returns_A_Valid_String_When_SendRequest_Is_Triggered_With_ReturnType_Of_GetPaymentResult_Then_It_Should_Deserialize_Correctly()
        {
            var httpProcessor = new HttpProcessor(_mockHttpClient.Object);
            var response = httpProcessor.SendRequest<object, RetrievePaymentResult>("", HttpMethod.Get, "https://SomeBaseURL.com").Result;
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(RetrievePaymentResult), response);
            Assert.AreEqual("********9012", response.CardNumber);
            Assert.AreEqual("Successful", response.Result);
        }
    }
}