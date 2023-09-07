using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Features.Payment.CommandModels;
using PaymentGateway.Application.Features.Payment.QueryModels;
using PaymentGateway.Application.Features.Payment.Services;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Api.Payments
{
    [ApiController]
    [Route("payments")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentGatewayController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        /// <summary>
        /// POST endpoint to process a payment
        /// </summary>
        /// <param name="payment">A payment with card, money and payer information</param>
        /// <returns>Status code</returns>
        [HttpPost("processPayment")]
        public async Task<IActionResult> Post([FromBody] ProcessPaymentCommand payment)
        {
            var paymentResponse = await _paymentService.ProcessPayment(payment);
            switch (paymentResponse.ResultCode)
            {
                case PaymentResult.InternalError:
                    var error = new BaseErrorResponse(StatusCodes.Status500InternalServerError,
                        "An error occurred during processing of request");
                    return StatusCode(StatusCodes.Status500InternalServerError,error);

            }

            return Ok(paymentResponse);
        }
        /// <summary>
        /// GET endpoint to retrieve a payment information
        /// </summary>
        /// <param name="paymentId">an integer representing Payment ID</param>
        /// <returns>Status Code</returns>
        [HttpGet("{paymentId?}")]
        public async Task<IActionResult> Get(string paymentId)
        {
            RetrievePaymentResult paymentResponse;
            try
            {
                paymentResponse = await _paymentService.RetrievePayment(paymentId);
            }
            catch (Exception)
            {
                var error = new BaseErrorResponse(StatusCodes.Status500InternalServerError,
                    "An error occurred during processing of request");
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            }
            return paymentResponse.Id==Guid.Empty
                ? NotFound($"Payment with Id {paymentId} Does not exist.")
                : Ok(paymentResponse);
        }
    }
}